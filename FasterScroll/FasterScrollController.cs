//#define DEBUG_FASTERSCROLL
using UnityEngine;
using HMUI;
using IPA.Utilities;
using VRUIControls;
using System.Linq;
using System.Collections;
using Libraries.HM.HMLib.VR;

namespace FasterScroll
{
    public class FasterScrollController : MonoBehaviour
    {
        public enum FasterScrollModeEnum
        {
            Constant,
            Linear,
            Exp,
            Stock
        }
        public enum RumbleModeEnum
        {
            Stock,
            Override,
            None
        }
        public static FasterScrollController Instance { get; private set; }
        public static FasterScrollModeEnum FasterScrollMode { get { return PluginSettings.instance.FasterScrollMode; } private set {} }
        public static float RumbleStrength { get { return m_fRumbleStrength; } private set { } }

        #region public
        /******************************
         *      Basic Unity Stuff     *
         ******************************/
        private void Awake()
        {
            if (Instance != null)
            {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;

            ResetInertia();
            Plugin.Log?.Debug($"{name}: Awake()");
#if DEBUG_FASTERSCROLL
            StartCoroutine(DebugUpdate());
#endif
        }

#if DEBUG_FASTERSCROLL
        private IEnumerator DebugUpdate()
        {
            while (true)
            {
                Plugin.Log?.Debug($"{name}:" +
                                  $" Faster Scroll Mode : {PluginSettings.instance.FasterScrollMode.ToString()} \n" +
                                  $" RumbleMode : {PluginSettings.instance.CustomRumbleMode.ToString()} \n" +
                                  $" Custom Rumble Strength : {PluginSettings.instance.CustomRumbleStrength} \n" +
                                  $" Scroll Acceleration : {PluginSettings.instance.Accel} \n" +
                                  $" Scroll Max Speed : {PluginSettings.instance.MaxSpeed} \n" +
                                  $" Inertia : {m_fInertia} \n" +
                                  $" Scroll Speed : {m_fCustomSpeed} \n" +
                                  $" Stock Scroll Speed : {m_fStockScrollSpeed} \n" +
                                  $" Actual Rumble Strength : {m_fRumbleStrength} \n" +
                                  $" Stock Rumble Strength : {m_fStockRumbleStrength} \n");
                yield return new WaitForSeconds(1.0f);
            }
        }
#endif

        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            songListScrollView.SetField("_joystickScrollSpeed", m_fStockScrollSpeed);
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.
        }

        /******************************
         *      Actual Fun stuff      *
         ******************************/

        // Postfix : ScrollView::Awake()
        public static void ScrollViewPatcherConstant(ScrollView sv)
        {
            if (sv.transform.parent.gameObject.name == "LevelsTableView")
            {
                sv.SetField("_joystickScrollSpeed", 300.0f);
                Plugin.Log?.Debug($"PATCHED CONSTANT _joystickScrollSpeed value : { sv.GetField<float, ScrollView>("_joystickScrollSpeed") }");
            }
        }

        public static void InitialSetup(ScrollView sv)
        {
            songListScrollView = sv;
            m_fStockScrollSpeed = sv.GetField<float, ScrollView>("_joystickScrollSpeed");
        }

        public static void SetStockRumbleStrength(VRInputModule vrinmod)
        {
            HapticPresetSO hapticPreset = vrinmod.GetField<HapticPresetSO, VRInputModule>("_rumblePreset");
            m_fStockRumbleStrength = hapticPreset._strength;
        }

        // Prefix : ScrollView::HandleJoystickWasNotCenteredThisFrame(Vector2 deltaPos)
        // Called every update when joystick is aiming at Song's list and its input != Vector2.zero 
        public static void ScrollViewPatcherDynamic(Vector2 deltaPos, ScrollView sv)
        {
            m_fScrollTimer += Time.deltaTime;
            switch(PluginSettings.instance.FasterScrollMode)
            {
                case FasterScrollModeEnum.Constant:
                {
                    m_fInertia = PluginSettings.instance.Accel* m_fScrollTimer;
                    break;
                }
                case FasterScrollModeEnum.Exp:
                {
                    m_fInertia = Mathf.Exp(PluginSettings.instance.Accel) * m_fScrollTimer;
                    break;
                }
                case FasterScrollModeEnum.Stock:
                case FasterScrollModeEnum.Linear:
                {
                    return;
                }
            }

            m_fCustomSpeed = Mathf.Clamp(m_fInertia * m_fStockScrollSpeed, 0.0f, PluginSettings.instance.MaxSpeed);
            sv.SetField("_joystickScrollSpeed", m_fCustomSpeed);
            //Plugin.Log?.Debug($"NEW VALUE : { sv.GetField<float, ScrollView>("_joystickScrollSpeed") }");
        }

        public static void ResetInertia() { m_fInertia = 0.0f; m_fScrollTimer = 0.0f; } // TODO check what happens when coming back from level with inertia
        #endregion public

        /******************************
         *        Rumble stuff        *
         ******************************/
        private static void SetHapticFeedbackController()
        {
            VRInputModule vrInputModule = Resources.FindObjectsOfTypeAll<VRInputModule>().FirstOrDefault();
            if (vrInputModule != null)
                m_oHaptic = vrInputModule.GetField<HapticFeedbackController, VRInputModule>("_hapticFeedbackController");
            else
                Plugin.Log?.Error($"Couldn't find HapticFeedbackController");
        }

        public static void PostHandlePointerDidEnter()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            switch (PluginSettings.instance.CustomRumbleMode)
            {
                case RumbleModeEnum.Override:
                {
                    m_fRumbleStrength = PluginSettings.instance.CustomRumbleStrength;
                    break;
                }
                case RumbleModeEnum.None:
                {
                    m_fRumbleStrength = 0.0f;
                    break;
                }
                case RumbleModeEnum.Stock:
                {
                    // TODO detect if nalulululuna's RumbleMod is installed, if so don't use m_fStockRumbleStrength
                    m_fRumbleStrength = m_fStockRumbleStrength; 
                    break;
                }
            }
        }

        public static void PostHandlePointerDidExit()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            m_fRumbleStrength = m_fStockRumbleStrength;
        }

#region private

        private static ScrollView songListScrollView;

        private static float m_fInertia;
        private static float m_fCustomSpeed; // stock value : 60.0f
        private static float m_fScrollTimer;
        private static float m_fStockScrollSpeed;

        private static HapticFeedbackController m_oHaptic;
        private static float m_fStockRumbleStrength; // stock value : 1.0f
        private static float m_fRumbleStrength;
        #endregion private
    }
}

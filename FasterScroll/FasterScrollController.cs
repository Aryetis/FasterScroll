using UnityEngine;
using HMUI;
using IPA.Utilities;
using VRUIControls;
using System.Linq;

namespace FasterScroll
{
    public class FasterScrollController : MonoBehaviour
    {
        public enum FasterScrollModeEnum
        {
            Constant,
            Linear,
            Exp,
            Default // TODO lol
        }
        public enum RumbleModeEnum
        {
            Default,
            Override,
            None
        }
        public static FasterScrollController Instance { get; private set; }
        public static FasterScrollModeEnum FasterScrollMode() { return PluginSettings.instance.FasterScrollMode; }
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
        }

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
                Destroy(sv.transform.parent.gameObject);
                sv.SetField("_joystickScrollSpeed", 300.0f);
                Plugin.Log?.Debug($"PATCHED CONSTANT _joystickScrollSpeed value  : { sv.GetField<float, ScrollView>("_joystickScrollSpeed") }");
            }
        }

        public static void InitialSetup(ScrollView sv)
        {
            songListScrollView = sv;
            m_fStockScrollSpeed = sv.GetField<float, ScrollView>("_joystickScrollSpeed");
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
                case FasterScrollModeEnum.Default:
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
            m_fStockRumbleStrength = 1.0f; // TODO actually get it jic
        }

        public static void PostHandlePointerDidEnter()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            switch (PluginSettings.instance.RumbleMode)
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
                case RumbleModeEnum.Default:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
            }
        }

        // TODO handle nalulululuna/RumbleMod compatibility => 
        // if detected use its Configuration.PluginConfig.Instance.strength_ui for RumbleModeEnum.Default
        public static void PostHandlePointerDidExit()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            switch (PluginSettings.instance.RumbleMode)
            {
                case RumbleModeEnum.Override :
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
                case RumbleModeEnum.None:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
                case RumbleModeEnum.Default:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
            }
        }

#region private

        private static ScrollView songListScrollView;

        private static float m_fInertia;
        private static float m_fCustomSpeed; // stock value : 60.0f;
        private static float m_fScrollTimer;
        private static float m_fStockScrollSpeed;

        private static HapticFeedbackController m_oHaptic;
        private static float m_fStockRumbleStrength;
        private static float m_fRumbleStrength;

        // TODO move everything in PluginSettings \/
        //private static float m_customRumbleStrength;// = 0.15f;
        //private static float m_fAccel;// = 1.0f;
        //private static float m_fMaxSpeed;// = 6000.0f;
        //private static RumbleMode m_eRumble;// = RumbleMode.None;
        //private static FasterScrollModeEnum m_eFasterScrollMode;// = FasterScrollModeEnum.Exp;
        #endregion private
    }
}

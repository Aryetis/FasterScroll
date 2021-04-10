#define DEBUG_FASTERSCROLL
using UnityEngine;
using HMUI;
using IPA.Utilities;
using VRUIControls;
using System.Linq;
using Libraries.HM.HMLib.VR;
using System;
using RumbleMod;

#if DEBUG_FASTERSCROLL
using System.Collections;
#endif

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
            Override,
            None,
            Stock
        }
        public static FasterScrollController Instance { get; private set; }
        public static FasterScrollModeEnum FasterScrollMode { get { return PluginConfig.Instance.FasterScrollMode; } private set {} }
        public static float RumbleStrength { get { return m_fRumbleStrength; } private set { } }
        public static bool IsRumbleStrengthValueDirty;
        public static bool NalunaRumbleModeDetected => IPA.Loader.PluginManager.EnabledPlugins.Any(x => x.Id == "RumbleMod");
        public static float StockRumbleStrength 
        {
            get
            {
                if (NalunaRumbleModeDetected)
                    return PersistentSingleton<SettingsController>.instance.strength_ui;
                else
                    return m_fStockRumbleStrength;
            }
            set { m_fStockRumbleStrength = value; }
        }


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

            IsRumbleStrengthValueDirty = false;
            ResetInertia();
            Plugin.Log?.Debug($"{name}: Awake()");
#if DEBUG_FASTERSCROLL
            //StartCoroutine(DebugUpdate());
#endif
        }

        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.
        }

        /*************************************************
         *      Initialization from Harmony Patches      *
         *************************************************/

        // No guarantee of getting stockScrollSpeed, depends of when it's called (meant to be called at BS launch-ish)
        public static void SetStockScrollSpeed(ScrollView sv)
        {
            m_fStockScrollSpeed = sv.GetField<float, ScrollView>("_joystickScrollSpeed");
        }

        // No guarantee of getting stockScrollSpeed, depends of when it's called (meant to be called on MainMenu)
        public static void SetStockRumbleStrength(float f)
        {
            if (NalunaRumbleModeDetected)
                m_fStockRumbleStrength = PersistentSingleton<SettingsController>.instance.strength_ui;
            else
                m_fStockRumbleStrength = f;
        }

        public static void ResetInertia()
        {
            m_fInertia = 0.0f; m_fScrollTimer = 0.0f;
        } // TODO check what happens when coming back from level with inertia

        /******************************
         *      Actual Fun stuff      *
         ******************************/

        public static void ScrollViewPatcherConstant(LevelCollectionTableView lctv)
        {
            TableView tv = lctv.GetField<TableView, LevelCollectionTableView>("_tableView");
            ScrollView sv = tv.GetComponent<ScrollView>();

            if (sv.transform.parent.gameObject.name == "LevelsTableView")
                sv.SetField("_joystickScrollSpeed", PluginConfig.Instance.MaxSpeed);
        }

        public static void ScrollViewPatcherStock(LevelCollectionTableView lctv)
        {
            TableView tv = lctv.GetField<TableView, LevelCollectionTableView>("_tableView");
            ScrollView sv = tv.GetComponent<ScrollView>();

            if (sv.transform.parent.gameObject.name == "LevelsTableView")
                sv.SetField("_joystickScrollSpeed", m_fStockScrollSpeed);
        }

        // Prefix : ScrollView::HandleJoystickWasNotCenteredThisFrame(Vector2 deltaPos)
        // Called every update when joystick is aiming at Song's list and its input != Vector2.zero 
        public static void ScrollViewPatcherDynamic(ScrollView sv)
        {
            m_fScrollTimer += Time.deltaTime;
            switch(PluginConfig.Instance.FasterScrollMode)
            {
                case FasterScrollModeEnum.Constant:
                {
                    m_fInertia = PluginConfig.Instance.Accel* m_fScrollTimer;
                    break;
                }
                case FasterScrollModeEnum.Exp:
                {
                    m_fInertia = Mathf.Exp(PluginConfig.Instance.Accel) * m_fScrollTimer;
                    break;
                }
                case FasterScrollModeEnum.Stock:
                case FasterScrollModeEnum.Linear:
                {
                    return;
                }
            }

            m_fCustomSpeed = Mathf.Clamp(m_fInertia * m_fStockScrollSpeed, 0.0f, PluginConfig.Instance.MaxSpeed);
            sv.SetField("_joystickScrollSpeed", m_fCustomSpeed);
        }
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

            switch (PluginConfig.Instance.CustomRumbleMode)
            {
                case RumbleModeEnum.Override:
                {
                    m_fRumbleStrength = PluginConfig.Instance.CustomRumbleStrength;
                    break;
                }
                case RumbleModeEnum.None:
                {
                    m_fRumbleStrength = 0.0f;
                    break;
                }
                case RumbleModeEnum.Stock:
                {
                    m_fRumbleStrength = StockRumbleStrength;
                    break;
                }
            }
            IsRumbleStrengthValueDirty = true;
        }

        public static void PostHandlePointerDidExit()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            m_fRumbleStrength = StockRumbleStrength;
            IsRumbleStrengthValueDirty = true;
        }

#region private
        private static float m_fInertia;
        private static float m_fCustomSpeed; // stock value : 60.0f
        private static float m_fScrollTimer;
        private static float m_fStockScrollSpeed;

        private static HapticFeedbackController m_oHaptic;
        private static float m_fStockRumbleStrength; // stock value : 1.0f (will be set ONCE at launch)
        private static float m_fRumbleStrength;
        #endregion private

        /******************************
         *         Debug stuff        *
         ******************************/
#region debug
#if DEBUG_FASTERSCROLL
        private IEnumerator DebugUpdate()
        {
            while (true)
            {
                Plugin.Log?.Debug($"{name}:\n" +
                                  $" Faster Scroll Mode : {PluginConfig.Instance.FasterScrollMode.ToString()} \n" +
                                  $" Scroll Acceleration : {PluginConfig.Instance.Accel} \n" +
                                  $" Scroll Max Speed : {PluginConfig.Instance.MaxSpeed} \n" +
                                  $" RumbleMode : {PluginConfig.Instance.CustomRumbleMode.ToString()} \n" +
                                  $" Custom Rumble Strength : {PluginConfig.Instance.CustomRumbleStrength} \n" +
                                  $" Inertia : {m_fInertia} \n" +
                                  $" Scroll Speed : {m_fCustomSpeed} \n" +
                                  $" Stock Scroll Speed : {m_fStockScrollSpeed} \n" +
                                  $" Actual Rumble Strength : {m_fRumbleStrength} \n" +
                                  $" Stock Rumble Strength : {StockRumbleStrength} \n");
                yield return new WaitForSeconds(1.0f);
            }
        }

        public static string GetGameObjectFullPath(GameObject go)
        {
            string path = "/" + go.name;
            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                path = "/" + go.name + path;
            }
            return path;
        }
    }
#endif
#endregion debug
}

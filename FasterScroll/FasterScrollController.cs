using UnityEngine;
using HMUI;
using IPA.Utilities;
using VRUIControls;
using System.Linq;

namespace FasterScroll
{
    public class FasterScrollController : MonoBehaviour
    {
        public static FasterScrollController Instance { get; private set; }
        public static bool IsAccelMode() { return m_bAccelMode; }
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

            Plugin.Log?.Debug($"{name}: Awake()");
        }

        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            songListScrollView.SetField("_joystickScrollSpeed", 300.0f);
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

        private static void SetHapticFeedbackController()
        {
            VRInputModule vrInputModule = Resources.FindObjectsOfTypeAll<VRInputModule>().FirstOrDefault();
            if (vrInputModule != null)
                m_oHaptic = vrInputModule.GetField<HapticFeedbackController, VRInputModule>("_hapticFeedbackController");
            else
                Plugin.Log?.Error($"Couldn't find HapticFeedbackController");
            m_fStockRumbleStrength = 1.0f; // TODO actually get it jic
        }

        // Prefix : ScrollView::HandleJoystickWasNotCenteredThisFrame(Vector2 deltaPos)
        // Called every update when joystick is aiming at Song's list and its input != Vector2.zero 
        public static void ScrollViewPatcherDynamic(Vector2 deltaPos, ScrollView sv)
        {
            m_fScrollTimer += Time.deltaTime;
            m_fInertia = (m_bExponentialInertia) 
                ?  Mathf.Exp(m_fAccel) * m_fScrollTimer
                : m_fAccel * m_fScrollTimer;
            m_fCustomSpeed = Mathf.Clamp(m_fInertia * m_fStockScrollSpeed, 0.0f, m_fMaxSpeed);
            sv.SetField("_joystickScrollSpeed", m_fCustomSpeed);
            //Plugin.Log?.Debug($"NEW VALUE : { sv.GetField<float, ScrollView>("_joystickScrollSpeed") }");
        }

        // Debug stuff
        //public static string GetFullPath(Transform current)
        //{
        //    if (current.parent == null)
        //        return "/" + current.name;
        //    return current.parent.GetPath() + "/" + current.name;
        //}

        public static void ResetInertia() { m_fInertia = 0.0f; m_fScrollTimer = 0.0f; }
#endregion public
        
        /******************************
         *        Rumble stuff        *
         ******************************/
        public static void PostHandlePointerDidEnter()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            switch (m_eRumble)
            {
                case RumbleMode.Override:
                {
                    m_fRumbleStrength = 0.15f;
                    break;
                }
                case RumbleMode.None:
                {
                    m_fRumbleStrength = 0.0f;
                    break;
                }
                case RumbleMode.Default:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
            }
        }

        // TODO handle nalulululuna/RumbleMod  
        public static void PostHandlePointerDidExit()
        {
            if (m_oHaptic == null)
                SetHapticFeedbackController();

            switch (m_eRumble)
            {
                case RumbleMode.Override :
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
                case RumbleMode.None:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
                case RumbleMode.Default:
                {
                    m_fRumbleStrength = m_fStockRumbleStrength;
                    break;
                }
            }
        }

#region private
        private enum RumbleMode
        {
            Default,
            Override,
            None
        }

        private static ScrollView songListScrollView;

        private static float m_fInertia = 0.0f;
        private static float m_fCustomSpeed = 300.0f; // 60.0f; // stock value
        private static float m_fMaxSpeed = 6000.0f;
        private static float m_fAccel = 1.0f;
        private static float m_fScrollTimer = 0.0f;
        private static float m_fStockScrollSpeed;
        private static bool m_bAccelMode = true;
        private static bool m_bExponentialInertia = true;

        private static RumbleMode m_eRumble = RumbleMode.None;
        private static HapticFeedbackController m_oHaptic;
        private static float m_fStockRumbleStrength;
        private static float m_fRumbleStrength = 1.0f;
#endregion private
    }
}

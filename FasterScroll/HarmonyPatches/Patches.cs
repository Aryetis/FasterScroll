using HarmonyLib;
using HMUI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRUIControls;
using Libraries.HM.HMLib.VR;

// TODO reapply all harmony patches upon settings modifications ?
namespace FasterScroll.Patches
{
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("Awake")]
    class ScrollViewAwakePatch
    {
        static void Prefix(ScrollView __instance)
        {
            // only interested in modifying :
            // Wrapper/ScreenSystem/ScreenContainer/MainScreen/LevelSelectionNavigationController/LevelCollectionNavigationController/LevelCollecionViewController/LevelsTableView/TableView
            if (__instance.transform.parent.gameObject.name == "LevelsTableView")
            {
                FasterScrollController.InitialSetup(__instance);

                if (FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Constant)
                    FasterScrollController.ScrollViewPatcherConstant(__instance);
            }
            return;
        }
    }

    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandleJoystickWasNotCenteredThisFrame")]
    class ScrollViewHandleJoystickWasNotCenteredThisFramePostfixPatch
    {
        static void Prefix(ScrollView __instance, Vector2 deltaPos)
        {
            if ((FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp
                    || FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear
                  ) && __instance.transform.parent.gameObject.name == "LevelsTableView")
            {
                FasterScrollController.ScrollViewPatcherDynamic(deltaPos, __instance);
            }
        }
    }

    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandleJoystickWasCenteredThisFrame")]
    class ScrollViewHandleJoystickWasCenteredThisFramePostfixPatch
    {
        static void Prefix(ScrollView __instance)
        {
            if ((FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp
                    || FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear
                 ) && __instance.transform.parent.gameObject.name == "LevelsTableView")
            {
                FasterScrollController.ResetInertia();
            }
        }
    }

    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidEnter")]
    class ScrollViewHandlePointerDidEnterPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
            FasterScrollController.PostHandlePointerDidEnter();
        }
    }

    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidExit")]
    class ScrollViewHandlePointerDidExitPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
            FasterScrollController.PostHandlePointerDidExit();
        }
    }

    [HarmonyPatch(typeof(BaseInputModule))]
    [HarmonyPatch("OnEnable")]
    class VRInputModuleAwakePostFixPatch
    {
        static void Postfix(BaseInputModule __instance/*HapticPresetSO ____rumblePreset*/)
        {
            if (__instance is VRInputModule)
                FasterScrollController.SetStockRumbleStrength(__instance as VRInputModule);
        }
    }

    [HarmonyPatch(typeof(VRInputModule))]
    [HarmonyPatch("HandlePointerExitAndEnter")]
    class VRInputModuleHandlePointerExitAndEnterPreFixPatch
    {
        static void Prefix(HapticPresetSO ____rumblePreset, GameObject newEnterTarget)
        {
            ____rumblePreset._strength = FasterScrollController.RumbleStrength;
        }
    }
}
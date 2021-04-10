using HarmonyLib;
using HMUI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRUIControls;
using Libraries.HM.HMLib.VR;

// only interested in modifying ( and its ScrollSpeed):
// Wrapper/ScreenSystem/ScreenContainer/MainScreen/LevelSelectionNavigationController/LevelCollectionNavigationController/LevelCollecionViewController/LevelsTableView/TableView

namespace FasterScroll.Patches
{
    // At Launch-ish
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("Awake")]
    class ScrollViewAwakePatch
    {
        static void Prefix(ScrollView __instance)
        {
            if (__instance.transform.parent.gameObject.name == "LevelsTableView")
                FasterScrollController.SetStockScrollSpeed(__instance); 
            return;
        }
    }

    // When Enabling SongListView
    [HarmonyPatch(typeof(LevelCollectionTableView))]
    [HarmonyPatch("OnEnable")]
    class TLevelCollectionTableViewOnEnablePostFixPatch
    {
        static void Postfix(LevelCollectionTableView __instance)
        {
            if (FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Constant)
                FasterScrollController.ScrollViewPatcherConstant(__instance);
            else if (FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Stock)
                FasterScrollController.ScrollViewPatcherStock(__instance); // Repatching to stock if previously was on Constant
        }
    }

    /******************************
     *       Set ScrollSpeed      *
     ******************************/

    // when pushing joystick on SongList
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandleJoystickWasNotCenteredThisFrame")]
    class ScrollViewHandleJoystickWasNotCenteredThisFramePostfixPatch
    {
        static void Prefix(ScrollView __instance, Vector2 deltaPos)
        {
            if ((FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp
                    || FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear
                  ) && __instance.transform.parent.gameObject.name == "LevelsTableView")
                FasterScrollController.ScrollViewPatcherDynamic(__instance);
        }
    }

    // when releasing / not pushing joystick on SongList
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandleJoystickWasCenteredThisFrame")]
    class ScrollViewHandleJoystickWasCenteredThisFramePostfixPatch
    {
        static void Prefix(ScrollView __instance)
        {
            if ((FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp
                    || FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear
                 ) && __instance.transform.parent.gameObject.name == "LevelsTableView")
                FasterScrollController.ResetInertia();
        }
    }

    /******************************
     *         Set Rumble         *
     ******************************/
    // When pointer enters Songlist's ScrollView
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidEnter")]
    class ScrollViewHandlePointerDidEnterPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
            if (__instance.transform.parent.gameObject.name == "LevelsTableView")
                FasterScrollController.PostHandlePointerDidEnter();
        }
    }

    // When pointer exits Songlist's ScrollView
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidExit")]
    class ScrollViewHandlePointerDidExitPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
            if (__instance.transform.parent.gameObject.name == "LevelsTableView")
                FasterScrollController.PostHandlePointerDidExit();
        }
    }

    [HarmonyPatch(typeof(VRInputModule))]
    [HarmonyPatch("HandlePointerExitAndEnter")]
    class VRInputModuleHandlePointerExitAndEnterPreFixPatch
    {
        static void Prefix(HapticPresetSO ____rumblePreset)
        {
            if (FasterScrollController.IsRumbleStrengthValueDirty)
            {
                ____rumblePreset._strength = FasterScrollController.RumbleStrength;
                FasterScrollController.IsRumbleStrengthValueDirty = false;
            }
        }
    }
}
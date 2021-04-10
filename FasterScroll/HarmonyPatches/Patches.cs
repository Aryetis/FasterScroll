﻿using HarmonyLib;
using HMUI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRUIControls;
using Libraries.HM.HMLib.VR;

// TODO reapply all harmony patches upon settings modifications ? 
// Probably shouldn't have to do that if organized properly, let's trace stuff
// TODO test all that with vanilla songList ... without songBrowser... hopefuly stuff shouldn't be that different
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
Plugin.Log?.Error("ScrollViewAwakePatch");
                FasterScrollController.SetStockScrollSpeed(__instance);
            }
            return;
        }
    }

    [HarmonyPatch(typeof(BaseInputModule))]
    [HarmonyPatch("OnEnable")]
    class VRInputModuleAwakePostFixPatch
    {
        static void Postfix(BaseInputModule __instance)
        {
            if (__instance is VRInputModule)
            {
                Plugin.Log?.Error("VRInputModuleAwakePostFixPatch");
                // TODO will probably be called only once => will not intercept changes on UI strength from RumbleMod
                FasterScrollController.SetStockRumbleStrength(__instance as VRInputModule);
            }
        }
    }


    [HarmonyPatch(typeof(LevelCollectionTableView))]
    [HarmonyPatch("OnEnable")]
    class TLevelCollectionTableViewOnEnablePostFixPatch
    {
        static void Postfix(LevelCollectionTableView __instance)
        {
Plugin.Log?.Error("TLevelCollectionTableViewOnEnablePostFixPatch");
            if (FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Constant)
                FasterScrollController.ScrollViewPatcherConstant(__instance);
            if (FasterScrollController.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Stock)
                FasterScrollController.ScrollViewPatcherStock(__instance); // Repatching to stock if previously was on Constant
        }
    }

    /******************************
     *       Set ScrollSpeed      *
     ******************************/

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
//Plugin.Log?.Error("ScrollViewHandleJoystickWasNotCenteredThisFramePostfixPatch");
                FasterScrollController.ScrollViewPatcherDynamic(__instance);
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
//Plugin.Log?.Error("ScrollViewHandleJoystickWasCenteredThisFramePostfixPatch");
                FasterScrollController.ResetInertia();
            }
        }
    }

    /******************************
     *         Set Rumble         *
     ******************************/
    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidEnter")]
    class ScrollViewHandlePointerDidEnterPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
//Plugin.Log?.Error("ScrollViewHandlePointerDidEnterPostFixPatch");
            FasterScrollController.PostHandlePointerDidEnter();
        }
    }

    [HarmonyPatch(typeof(ScrollView))]
    [HarmonyPatch("HandlePointerDidExit")]
    class ScrollViewHandlePointerDidExitPostFixPatch
    {
        static void Prefix(ScrollView __instance, PointerEventData eventData)
        {
//Plugin.Log?.Error("ScrollViewHandlePointerDidExitPostFixPatch");
            FasterScrollController.PostHandlePointerDidExit();
        }
    }

    [HarmonyPatch(typeof(VRInputModule))]
    [HarmonyPatch("HandlePointerExitAndEnter")]
    class VRInputModuleHandlePointerExitAndEnterPreFixPatch
    {
        static void Prefix(HapticPresetSO ____rumblePreset)
        {
            if (FasterScrollController.IsRumbleDirty)
            {
//Plugin.Log?.Error("VRInputModuleHandlePointerExitAndEnterPreFixPatch");
                ____rumblePreset._strength = FasterScrollController.RumbleStrength;
                FasterScrollController.IsRumbleDirty = false;
            }
        }
    }
}
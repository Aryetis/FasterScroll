using System;
using System.Reflection;
using IPA;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using BeatSaberMarkupLanguage.Settings;
using IPA.Config;
using IPA.Config.Stores;

namespace FasterScroll
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public const string HarmonyId = "com.github.Aryetis.FasterScroll";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);

        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Config conf)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");
            PluginConfig.Instance = conf.Generated<PluginConfig>();
        }

#region Disableable
        [OnStart]
        public void OnApplicationStart()
        {
            BSMLSettings.instance.AddSettingsMenu("FasterScroll", "FasterScroll.Views.Settings.bsml", PluginSettings.instance);
            new GameObject("FasterScrollController").AddComponent<FasterScrollController>();
            ApplyHarmonyPatches();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            BSMLSettings.instance.RemoveSettingsMenu(PluginSettings.instance);
            RemoveHarmonyPatches();
        }
#endregion

#region Harmony
        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            try
            {
                // Removes all patches with this HarmonyId
                harmony.UnpatchAll(HarmonyId);
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }
#endregion
    }
}

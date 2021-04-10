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
    [Plugin(RuntimeOptions.DynamicInit)] // TODO turn into nondynamic ... otherwise it will be a pain in the butt to reset potential tweaked scrollvalue 
    public class Plugin
    {
        public const string HarmonyId = "com.github.Aryetis.FasterScroll";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);

        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static FasterScrollController PluginController { get { return FasterScrollController.Instance; } }

        [Init]
        public Plugin(IPALogger logger, Config conf)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");
            PluginConfig.Instance = conf.Generated<PluginConfig>();
        }

#region Disableable
        [OnEnable]
        public void OnEnable()
        {
            BSMLSettings.instance.AddSettingsMenu("FasterScroll", "FasterScroll.Views.Settings.bsml", PluginSettings.instance);
            new GameObject("FasterScrollController").AddComponent<FasterScrollController>();
            ApplyHarmonyPatches();
        }

        [OnDisable]
        public void OnDisable()
        {
            BSMLSettings.instance.RemoveSettingsMenu(PluginSettings.instance);
            if (PluginController != null)
                GameObject.Destroy(PluginController);
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

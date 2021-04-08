using BeatSaberMarkupLanguage.Attributes;
using IPA.Config.Stores;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace FasterScroll
{
    class PluginSettings : PersistentSingleton<PluginSettings>
    {
        [UIValue("CustomRumbleStrength")]
        public float CustomRumbleStrength { get; set; } = 0.15f;

        [UIValue("Accel")]
        public float Accel { get; set; } = 1.0f;

        [UIValue("MaxSpeed")]
        public float MaxSpeed { get; set; } = 3000.00f;

        [UIValue("CustomRumbleModeOptions")]
        private System.Collections.Generic.List<object> RumbleModeOptions
                = new object[] { "Default", "Override", "None" }.ToList();

        [UIValue("CustomRumbleModeString")]
        public string CustomRumbleModeString { get; set; } = "Override";
        public FasterScrollController.RumbleModeEnum CustomRumbleMode { get; set; }

        [UIValue("FasterScrollModeOptions")]
        private System.Collections.Generic.List<object> FasterScrollModeOptions
            = new object[] { "Constant", "Linear", "Exp", "Stock" }.ToList();

        [UIValue("FasterScrollModeString")]
        public string FasterScrollModeString { get; set; } = "Exp";
        public FasterScrollController.FasterScrollModeEnum FasterScrollMode { get; set; }

        [UIAction("#apply")]
        public void OnApply()
        {
            for (int i = 0; i < RumbleModeOptions.Count; i++)
            {
                if (CustomRumbleModeString == RumbleModeOptions[i] as string)
                {
                    CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                    break;
                }
            }

            for (int i = 0; i < FasterScrollModeOptions.Count; i++)
            {
                if (FasterScrollModeString == FasterScrollModeOptions[i] as string)
                {
                    FasterScrollMode = (FasterScrollController.FasterScrollModeEnum)i;
                    break;
                }
            }
        }
    }
}
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
            
        [UIValue("CustomRumbleMode")]
        public FasterScrollController.RumbleModeEnum RumbleMode { get; set; } 
                            = FasterScrollController.RumbleModeEnum.Override;

        [UIValue("FasterScrollModeOptions")]
        private System.Collections.Generic.List<object> FasterScrollModeOptions
            = new object[] { "Constant", "Linear", "Exp", "Stock" }.ToList();

        [UIValue("FasterScrollMode")]
        public FasterScrollController.FasterScrollModeEnum FasterScrollMode { get; set; } 
                                            = FasterScrollController.FasterScrollModeEnum.Exp;
    }
}
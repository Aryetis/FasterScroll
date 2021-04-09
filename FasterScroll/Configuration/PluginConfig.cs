using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace FasterScroll
{
    class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual float CustomRumbleStrength { get; set; } = 0.15f;
        public virtual float Accel { get; set; } = 1.0f;
        public virtual float MaxSpeed { get; set; } = 3000.00f;
        public virtual FasterScrollController.RumbleModeEnum CustomRumbleMode { get; set; } 
                                            = FasterScrollController.RumbleModeEnum.Override;
        public virtual FasterScrollController.FasterScrollModeEnum FasterScrollMode { get; set; } 
                                                = FasterScrollController.FasterScrollModeEnum.Exp;
    }
}

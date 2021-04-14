using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace FasterScroll
{
    class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual FasterScrollController.FasterScrollModeEnum FasterScrollMode { get; set; } = DefaultFasterScrollMode;
        public virtual float Accel { get; set; } = DefaultAccel;
        public virtual float MaxSpeed { get; set; } = DefaultMaxSpeed;
        public virtual FasterScrollController.RumbleModeEnum CustomRumbleMode { get; set; } = DefaultCustomRumbleMode;
        public virtual float CustomRumbleStrength { get; set; } = DefaultCustomRumbleStrength;

        /////////////////////////////////////////////
        // Because C# is stupid and doesn't allow global variables nor have proper macros, let's dump those here <3
        public const FasterScrollController.FasterScrollModeEnum DefaultFasterScrollMode
                                        = FasterScrollController.FasterScrollModeEnum.Exp;
        public const float DefaultAccel = 1.0f;
        public const float DefaultMaxSpeed = 600.00f;
        public const FasterScrollController.RumbleModeEnum DefaultCustomRumbleMode = FasterScrollController.RumbleModeEnum.Override;
        public const float DefaultCustomRumbleStrength = 0.15f;
        /////////////////////////////////////////////
    }
}

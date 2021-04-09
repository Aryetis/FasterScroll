using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace FasterScroll
{
    public class PluginSettings : PersistentSingleton<PluginSettings>
    {
        [UIValue("CustomRumbleStrength")]
        public float m_fCustomRumbleStrength { get; set; }

        [UIValue("Accel")]
        public float m_fAccel { get; set; }

        [UIValue("MaxSpeed")]
        public float m_fMaxSpeed { get; set; }

        [UIValue("CustomRumbleModeOptions")]
        public List<object> m_lCustomRumbleModeOptions = new object[] { "Stock", "Override", "None" }.ToList();
        [UIValue("CustomRumbleModeString")]
        public string m_sCustomRumbleModeString { get; set; }

        [UIValue("FasterScrollModeOptions")]
        public List<object> m_lFasterScrollModeOptions = new object[] { "Constant", "Linear", "Exp", "Stock" }.ToList();
        [UIValue("FasterScrollModeString")]
        public string m_sFasterScrollModeString { get; set; }

        PluginSettings()
        {
            m_fCustomRumbleStrength = PluginConfig.Instance.CustomRumbleStrength;
            m_fAccel = PluginConfig.Instance.Accel;
            m_fMaxSpeed = PluginConfig.Instance.MaxSpeed;
            m_sCustomRumbleModeString = System.Enum.GetName(typeof(FasterScrollController.RumbleModeEnum)
                                                            , PluginConfig.Instance.CustomRumbleMode);
            m_sFasterScrollModeString = System.Enum.GetName(typeof(FasterScrollController.FasterScrollModeEnum)
                                                            , PluginConfig.Instance.FasterScrollMode);
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            PluginConfig.Instance.CustomRumbleStrength = m_fCustomRumbleStrength;
            PluginConfig.Instance.Accel = m_fAccel;
            PluginConfig.Instance.MaxSpeed = m_fMaxSpeed;

            for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
            {
                if (m_sCustomRumbleModeString == m_lCustomRumbleModeOptions[i] as string)
                {
                    PluginConfig.Instance.CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                    break;
                }
            }

            for (int i = 0; i < m_lFasterScrollModeOptions.Count; i++)
            {
                if (m_sFasterScrollModeString == m_lFasterScrollModeOptions[i] as string)
                {
                    PluginConfig.Instance.FasterScrollMode = (FasterScrollController.FasterScrollModeEnum)i;
                    break;
                }
            }
        }
    }
}
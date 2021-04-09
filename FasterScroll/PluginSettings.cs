using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Components;

namespace FasterScroll
{
    public class PluginSettings : NotifiableSingleton<PluginSettings>
    {
        [UIValue("FasterScrollModeOptions")]
        private List<object> m_lFasterScrollModeOptions = new object[] { "Constant", "Linear", "Exp", "Stock" }.ToList();
        [UIValue("FasterScrollModeString")]
        private string m_sFasterScrollModeString { get; set; }
        [UIAction("FasterScrollModeStringUpdate")]
        private void FasterScrollModeStringUpdate(string newVal)
        {
            for (int i = 0; i < m_lFasterScrollModeOptions.Count; i++)
            {
                if (newVal == m_lFasterScrollModeOptions[i] as string)
                {
                    PluginConfig.Instance.FasterScrollMode = (FasterScrollController.FasterScrollModeEnum)i;
                    showAccel = (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp)
                             || (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear);
                    showMaxSpeed = (PluginConfig.Instance.FasterScrollMode != FasterScrollController.FasterScrollModeEnum.Stock);
                    break;
                }
            }
        }

        // (Stock || Constant) Scroll => Hide Accel
        [UIValue("showAccel")]
        private bool showAccel
        {
            get => (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp)
                   || (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("Accel")]
        private float m_fAccel { get; set; }

        // Stock Scroll => Hide MaxSpeed
        [UIValue("showMaxSpeed")]
        private bool showMaxSpeed
        {
            get => (PluginConfig.Instance.FasterScrollMode != FasterScrollController.FasterScrollModeEnum.Stock);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("MaxSpeed")]
        private float m_fMaxSpeed { get; set; }

        // RumbleMode Override => Hide RumbleStrength
        [UIValue("CustomRumbleModeOptions")]
        private List<object> m_lCustomRumbleModeOptions = new object[] { "Stock", "Override", "None" }.ToList();
        [UIValue("CustomRumbleModeString")]
        private string CustomRumbleModeString { get; set; }
        [UIAction("CustomRumbleModeStringUpdate")]
        private void CustomRumbleModeStringUpdate(string newVal)
        {
            for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
            {
                if (newVal == m_lCustomRumbleModeOptions[i] as string)
                {
                    PluginConfig.Instance.CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                    showCustomRumbleStrength = (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
                    break;
                }
            }
        }

        [UIValue("showCustomRumbleStrength")]
        private bool showCustomRumbleStrength
        {
            get => (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("CustomRumbleStrength")]
        private float m_fCustomRumbleStrength { get; set; }

        PluginSettings()
        {
            m_fCustomRumbleStrength = PluginConfig.Instance.CustomRumbleStrength;
            m_fAccel = PluginConfig.Instance.Accel;
            m_fMaxSpeed = PluginConfig.Instance.MaxSpeed;
            CustomRumbleModeString = System.Enum.GetName(typeof(FasterScrollController.RumbleModeEnum)
                                                            , PluginConfig.Instance.CustomRumbleMode);
            m_sFasterScrollModeString = System.Enum.GetName(typeof(FasterScrollController.FasterScrollModeEnum)
                                                            , PluginConfig.Instance.FasterScrollMode);
        }

        [UIAction("#apply")]
        private void OnApply()
        {
            PluginConfig.Instance.CustomRumbleStrength = m_fCustomRumbleStrength;
            PluginConfig.Instance.Accel = m_fAccel;
            PluginConfig.Instance.MaxSpeed = m_fMaxSpeed;

            for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
            {
                if (CustomRumbleModeString == m_lCustomRumbleModeOptions[i] as string)
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
using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;

namespace FasterScroll
{
    public class PluginSettings : NotifiableSingleton<PluginSettings>
    {
        [UIParams]
        private BSMLParserParams m_oParserParams;

        [UIValue("FasterScrollModeOptions")]
        private List<object> m_lFasterScrollModeOptions
            = (System.Enum.GetNames(typeof(FasterScrollController.FasterScrollModeEnum))).OfType<object>().ToList();
        [UIValue("FasterScrollModeString")]
        private string m_sFasterScrollModeString 
        {
            get
            {
                return System.Enum.GetName(typeof(FasterScrollController.FasterScrollModeEnum)
                                                , PluginConfig.Instance.FasterScrollMode);
            }
            set
            {
                for (int i = 0; i < m_lFasterScrollModeOptions.Count; i++)
                {
                    if (value == m_lFasterScrollModeOptions[i] as string)
                    {
                        PluginConfig.Instance.FasterScrollMode = (FasterScrollController.FasterScrollModeEnum)i;
                        break;
                    }
                }
                NotifyPropertyChanged();
            }
        }
        [UIAction("FasterScrollModeStringUpdate")]
        private void FasterScrollModeStringUpdate(string newVal)
        {
            for (int i = 0; i < m_lFasterScrollModeOptions.Count; i++)
            {
                if (newVal == m_lFasterScrollModeOptions[i] as string)
                {
                    PluginConfig.Instance.FasterScrollMode = (FasterScrollController.FasterScrollModeEnum)i;
                    m_bShowAccel = (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp)
                             || (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear);
                    m_bShowMaxSpeed = (PluginConfig.Instance.FasterScrollMode != FasterScrollController.FasterScrollModeEnum.Stock);
                    break;
                }
            }
        }

        // (Stock || Constant) Scroll => Hide Accel
        [UIValue("showAccel")]
        private bool m_bShowAccel
        {
            get => (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Exp)
                   || (PluginConfig.Instance.FasterScrollMode == FasterScrollController.FasterScrollModeEnum.Linear);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("Accel")]
        private float m_fAccel 
        { 
            get => PluginConfig.Instance.Accel;
            set { PluginConfig.Instance.Accel = value; NotifyPropertyChanged(); }
        }

        // Stock Scroll => Hide MaxSpeed
        [UIValue("showMaxSpeed")]
        private bool m_bShowMaxSpeed
        {
            get => (PluginConfig.Instance.FasterScrollMode != FasterScrollController.FasterScrollModeEnum.Stock);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("MaxSpeed")]
        private float m_fMaxSpeed
        {
            get => PluginConfig.Instance.MaxSpeed;
            set { PluginConfig.Instance.MaxSpeed = value; NotifyPropertyChanged(); } 
        }

        // RumbleMode Override => Hide RumbleStrength
        [UIValue("CustomRumbleModeOptions")]
        private List<object> m_lCustomRumbleModeOptions
            = (System.Enum.GetNames(typeof(FasterScrollController.RumbleModeEnum))).OfType<object>().ToList();

        [UIValue("CustomRumbleModeString")]
        private string m_sCustomRumbleModeString
        { 
            get
            {
                return System.Enum.GetName(typeof(FasterScrollController.RumbleModeEnum)
                                                , PluginConfig.Instance.CustomRumbleMode);
            }
            set
            {
                for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
                {
                    if (value == m_lCustomRumbleModeOptions[i] as string)
                    {
                        PluginConfig.Instance.CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                        break;
                    }
                }
                NotifyPropertyChanged();
            }
        }
        [UIAction("CustomRumbleModeStringUpdate")]
        private void CustomRumbleModeStringUpdate(string newVal)
        {
            for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
            {
                if (newVal == m_lCustomRumbleModeOptions[i] as string)
                {
                    PluginConfig.Instance.CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                    m_bShowCustomRumbleStrength = (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
                    break;
                }
            }
        }

        [UIValue("showCustomRumbleStrength")]
        private bool m_bShowCustomRumbleStrength
        {
            get => (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
            set { NotifyPropertyChanged(); }
        }
        [UIValue("CustomRumbleStrength")]
        private float m_fCustomRumbleStrength
        {
            get => PluginConfig.Instance.CustomRumbleStrength;
            set { PluginConfig.Instance.CustomRumbleStrength = value; NotifyPropertyChanged(); }
        }

        [UIAction("ResetSettingsClicked")]
        private void ResetSettingsClicked()
        {
            m_sFasterScrollModeString = System.Enum.GetName(typeof(FasterScrollController.FasterScrollModeEnum), PluginConfig.DefaultFasterScrollMode);
            m_fAccel = PluginConfig.DefaultAccel;
            m_bShowAccel = true;
            m_fMaxSpeed = PluginConfig.DefaultMaxSpeed;
            m_bShowMaxSpeed = true;

            m_sCustomRumbleModeString = System.Enum.GetName(typeof(FasterScrollController.RumbleModeEnum), PluginConfig.DefaultCustomRumbleMode);
            m_fCustomRumbleStrength = PluginConfig.DefaultCustomRumbleStrength;
            m_bShowCustomRumbleStrength = true;
            m_oParserParams.EmitEvent("cancel");
        }
    }
}
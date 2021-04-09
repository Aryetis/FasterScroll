using BeatSaberMarkupLanguage.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeatSaberMarkupLanguage.Components;

namespace FasterScroll
{
    public class PluginSettings : NotifiableSingleton<PluginSettings>
    {
        // TODO stock Scroll must hide Accel & MaxSpeed
        // TODO Constant Scroll must hide accel
        [UIValue("FasterScrollModeOptions")]
        public List<object> m_lFasterScrollModeOptions = new object[] { "Constant", "Linear", "Exp", "Stock" }.ToList();
        [UIValue("FasterScrollModeString")]
        public string m_sFasterScrollModeString { get; set; }

        [UIValue("Accel")]
        public float m_fAccel { get; set; }

        [UIValue("MaxSpeed")]
        public float m_fMaxSpeed { get; set; }

        // TODO NOW fix dynamic menu stuttering ... Need to select twice the option for it to hide menus accordingly
        [UIValue("CustomRumbleModeOptions")]
        public List<object> m_lCustomRumbleModeOptions = new object[] { "Stock", "Override", "None" }.ToList();
        [UIValue("CustomRumbleModeString")]
        public string CustomRumbleModeString { get; set; }
        [UIAction("CustomRumbleModeStringUpdate")]
        private void CustomRumbleModeStringUpdate(string _)
        {
            Plugin.Log?.Error($"new rumble mode : {_}");
            for (int i = 0; i < m_lCustomRumbleModeOptions.Count; i++)
            {
                if (CustomRumbleModeString == m_lCustomRumbleModeOptions[i] as string)
                {
                    PluginConfig.Instance.CustomRumbleMode = (FasterScrollController.RumbleModeEnum)i;
                    showCustomRumbleStrength = (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
                    break;
                }
            }
        }

        [UIValue("showCustomRumbleStrength")]
        public bool showCustomRumbleStrength
        {
            get => (PluginConfig.Instance.CustomRumbleMode == FasterScrollController.RumbleModeEnum.Override);
            set { NotifyPropertyChanged(); Plugin.Log?.Error($"Setting showCustomRumbleStrength at : {value}"); }
        }
        [UIValue("CustomRumbleStrength")]
        public float m_fCustomRumbleStrength { get; set; }

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
        public void OnApply()
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
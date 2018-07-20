using BeatSaberKeyboardMapperPlugin.UI.Plugin.SettingsControllers;
using BeatSaberKeyboardMapperPlugin.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRUI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin.ViewControllers
{
    class SettingsListViewController : VRUIViewController
    {
        public List<SimpleSettingsController> settingControllers = new List<SimpleSettingsController>();

        public void Awake()
        {
            //settingControllers.Add(ReflectionUtil.CopySwitchSettingsController<EnabledSettingController>("Enabled", transform));
            
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (activationType == ActivationType.AddedToHierarchy)
            {
                base.DidActivate(firstActivation, activationType);
                if (firstActivation)
                {
                    foreach (SimpleSettingsController simpleSettingsController in settingControllers)
                    {
                        simpleSettingsController.Init();
                    }
                }
            }
        }

        public void ApplySettings()
        {
            foreach (SimpleSettingsController simpleSettingsController in settingControllers)
            {
                simpleSettingsController.ApplySettings();
            }
        }
    }
}

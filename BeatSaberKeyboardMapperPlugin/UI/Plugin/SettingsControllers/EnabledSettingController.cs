using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin.SettingsControllers
{
    class EnabledSettingController : SwitchSettingsController
    {
        protected override void ApplyValue(bool value)
        {
            Settings.Enabled = value;
        }

        protected override bool GetInitValue()
        {
            return Settings.Enabled;
        }

        protected override string TextForValue(bool value)
        {
            return value ? "On" : "Off";
        }
    }
}

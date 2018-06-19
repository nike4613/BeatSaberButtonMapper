using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.SettingsControllers
{
    class EnableToggleSettingsController : SwitchSettingsController
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
            return value ? "ON" : "OFF";
        }
    }
}

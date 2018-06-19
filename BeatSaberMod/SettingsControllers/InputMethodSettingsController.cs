using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.SettingsControllers
{
    class InputMethodSettingsController : ListSettingsController
    {
        protected override void ApplyValue(int idx)
        {
            Settings.ControllerMode = (ControllerMode)idx;
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            idx = (int)Settings.ControllerMode;
            numberOfElements = 3; // hardcoded because meh
        }

        protected override string TextForValue(int idx)
        {
            return ((ControllerMode)idx).ToString();
        }
    }
}

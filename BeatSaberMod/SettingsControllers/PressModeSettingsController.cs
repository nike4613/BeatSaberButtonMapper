using BeatSaberMod.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.SettingsControllers
{
    class PressModeSettingsController : ListSettingsController
    {
        private void Setup()
        {
            values = Enum.GetValues(typeof(InputMode)).Cast<InputMode>().ToArray();
        }

        InputMode[] values;

        protected override void ApplyValue(int idx)
        {
            Settings.InputMode = values[idx];
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            Setup();

            numberOfElements = values.Length;
            idx = Array.IndexOf(values, Settings.InputMode);
            if (idx == -1) idx = 0;
        }

        protected override string TextForValue(int idx)
        {
            return values[idx].ToNiceName();
        }
    }
}

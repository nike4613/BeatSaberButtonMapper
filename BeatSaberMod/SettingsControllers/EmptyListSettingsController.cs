using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.SettingsControllers
{
    class EmptyListSettingsController : ListSettingsController
    {
        protected override void ApplyValue(int idx)
        {

        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            idx = 0;
            numberOfElements = 0;
        }

        protected override string TextForValue(int idx)
        {
            return "";
        }
    }
}

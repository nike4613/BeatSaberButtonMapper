using BeatSaberMod.IPA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod.SettingsControllers
{
    class BindingEditorSelectorSettingsController : ListSettingsController
    {
        protected override void ApplyValue(int idx)
        {

        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            idx = 0;
            numberOfElements = Settings.Bindings.Count + 1;
        }

        protected override string TextForValue(int idx)
        {
            if (idx == 0)
            {
                return "None";
            }

            Logging.log.Debug($"Selecting binding index {idx - 1}");
            BindingsConfigMainViewController.Instance._rightScreenController.SetSelectedBinding(idx - 1);

            return $"Binding {idx}";
        }
    }
}

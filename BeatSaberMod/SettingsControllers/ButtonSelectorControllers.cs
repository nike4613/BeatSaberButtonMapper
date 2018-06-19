using BeatSaberMod.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatSaberMod.SettingsControllers
{

    class ControllerButtonSelectorController : ListSettingsController
    {
        public int SelectedIndex = -1;

        private void Setup()
        {
            Type enumType;

            switch (Settings.ControllerMode)
            {
                case ControllerMode.Oculus:
                    enumType = typeof(ControllerInput.Oculus);
                    break;
                case ControllerMode.Vive:
                    enumType = typeof(ControllerInput.Vive);
                    break;
                case ControllerMode.WinMR:
                    enumType = typeof(ControllerInput.WinMR);
                    break;
                default: // default to Vive for no particular reason
                    enumType = typeof(ControllerInput.Vive);
                    break;
            }

            values = Enum.GetValues(enumType).OfType<KeyCode>().ToArray();
        }

        private string GetName(KeyCode val)
        {
            switch (Settings.ControllerMode)
            {
                case ControllerMode.Oculus:
                    return ((ControllerInput.Oculus)val).ToNiceName();
                case ControllerMode.Vive:
                    return ((ControllerInput.Vive)val).ToNiceName();
                case ControllerMode.WinMR:
                    return ((ControllerInput.WinMR)val).ToNiceName();
            }
            return "";
        }

        KeyCode[] values;

        protected override void ApplyValue(int idx)
        {
            Settings.Bindings[SelectedIndex].SourceKey = values[idx];
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            Setup();

            numberOfElements = values.Length;
            if (SelectedIndex != -1)
            {
                idx = Array.IndexOf(values, Settings.Bindings[SelectedIndex].SourceKey);
            }
            else
            {
                idx = 0;
            }
        }

        protected override string TextForValue(int idx)
        {
            return GetName(values[idx]);
        }
    }

    class KeyboardButtonSelectorController : ListSettingsController
    {
        public int SelectedIndex = -1;

        private void Setup()
        {
            values = Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToArray();
        }

        KeyCode[] values;

        protected override void ApplyValue(int idx)
        {
            Settings.Bindings[SelectedIndex].DestKey = values[idx];
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            Setup();

            numberOfElements = values.Length;
            if (SelectedIndex != -1)
            {
                idx = Array.IndexOf(values, Settings.Bindings[SelectedIndex].DestKey);
            }
            else
            {
                idx = 0;
            }
        }

        protected override string TextForValue(int idx)
        {
            return values[idx].ToNiceName();
        }
    }
}

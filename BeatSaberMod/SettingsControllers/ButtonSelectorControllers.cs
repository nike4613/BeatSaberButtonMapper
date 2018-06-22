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
            
            values = Enum.GetValues(enumType).Cast<KeyCode>().ToArray();
        }

        private string GetName(KeyCode val)
        {
            Console.WriteLine(Settings.ControllerMode);

            switch (Settings.ControllerMode)
            {
                case ControllerMode.Oculus:
                    Console.WriteLine((ControllerInput.Oculus)val);
                    return ((ControllerInput.Oculus)val).ToNiceName();
                case ControllerMode.Vive:
                    Console.WriteLine((ControllerInput.Vive)val);
                    return ((ControllerInput.Vive)val).ToNiceName();
                case ControllerMode.WinMR:
                    Console.WriteLine((ControllerInput.WinMR)val);
                    return ((ControllerInput.WinMR)val).ToNiceName();
            }
            return "";
        }

        KeyCode[] values;

        protected override void ApplyValue(int idx)
        {
            if (SelectedIndex != -1)
                Settings.Bindings[SelectedIndex].SourceKey = values[idx];
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            Setup();

            numberOfElements = values.Length;

            if (SelectedIndex != -1)
            {
                idx = Array.IndexOf(values, Settings.Bindings[SelectedIndex].SourceKey);
                if (idx == -1) idx = 0;
            }
            else
            {
                idx = 0;
                numberOfElements = 0;
            }
        }

        protected override string TextForValue(int idx)
        {
            if (SelectedIndex == -1 && idx == 0) return "";
            return GetName(values[idx]);
        }
    }

    class KeyboardButtonSelectorController : ListSettingsController
    {
        public int SelectedIndex = -1;

        private void Setup()
        {
            values = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
        }

        KeyCode[] values;

        protected override void ApplyValue(int idx)
        {
            if (SelectedIndex != -1)
                Settings.Bindings[SelectedIndex].DestKey = values[idx];
        }

        protected override void GetInitValues(out int idx, out int numberOfElements)
        {
            Setup();

            numberOfElements = values.Length;
            if (SelectedIndex != -1)
            {
                idx = Array.IndexOf(values, Settings.Bindings[SelectedIndex].DestKey);
                if (idx == -1) idx = 0;
            }
            else
            {
                idx = 0;
                numberOfElements = 0;
            }
        }

        protected override string TextForValue(int idx)
        {
            if (SelectedIndex == -1 && idx == 0) return "";
            return values[idx].ToNiceName();
        }
    }
}

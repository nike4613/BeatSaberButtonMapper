using BeatSaberMod.SettingsControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using VRUI;

namespace BeatSaberMod.IPA.UI
{
    class BindingsConfigMainViewController : VRUINavigationController
    {
        internal static BindingsConfigMainViewController Instance;

        internal class ConfigRightScreenViewController : VRUIViewController
        {
            internal void CreateSettingsControllers()
            {
                cntrlBtnSel = CopyListSettingsController<ControllerButtonSelectorController>("Map", transform);
                CopyListSettingsController<EmptyListSettingsController>("", transform); // spacer
                CopyListSettingsController<EmptyListSettingsController>("", transform); // spacer
                keybBtnSel = CopyListSettingsController<KeyboardButtonSelectorController>("to", transform);
            }

            int curSelectedBinding = -1;
            private ControllerButtonSelectorController cntrlBtnSel;
            private KeyboardButtonSelectorController keybBtnSel;

            public void ApplyBindingSettings()
            {
                if (curSelectedBinding != -1)
                {
                    cntrlBtnSel.ApplySettings();
                    keybBtnSel.ApplySettings();
                }
            }
            public int GetSelectedBinding() => curSelectedBinding;
            public void SetSelectedBinding(int index, bool noapply = false)
            {
                Console.WriteLine($"Selecting binding {index}");

                if (!noapply && index != curSelectedBinding)
                    ApplyBindingSettings();

                Console.WriteLine("Applied old settings");

                if (index == -1)
                {
                    cntrlBtnSel.gameObject.SetActive(false);
                    keybBtnSel.gameObject.SetActive(false);

                    Console.WriteLine("Disabled parent");
                }
                else
                {
                    cntrlBtnSel.gameObject.SetActive(true);
                    keybBtnSel.gameObject.SetActive(true);

                    Console.WriteLine("Enabled parent");
                    Console.WriteLine($"{index} != {curSelectedBinding} : {index != curSelectedBinding}");
                }

                if (index != curSelectedBinding)
                {
                    cntrlBtnSel.SelectedIndex = index;
                    cntrlBtnSel.Init();
                    Console.WriteLine("Inited cntrlBtnSel");
                    keybBtnSel.SelectedIndex = index;
                    keybBtnSel.Init();
                    Console.WriteLine("Inited keybBtnSel");
                }

                curSelectedBinding = index;
            }
        }

        class NullViewController : VRUIViewController
        {

        }

        internal ConfigRightScreenViewController _rightScreenController;
        NullViewController _leftScreenController;

        SimpleSettingsController[] _settingControllers;

        protected override void DidActivate(bool first, ActivationType activationType)
        {
            Instance = this;

            if (_rightScreenController == null)
            {
                _rightScreenController = BeatSaberUI.CreateViewController<ConfigRightScreenViewController>();
                _rightScreenController.rectTransform.anchorMin = new Vector2(0.3f, 0f);
                _rightScreenController.rectTransform.anchorMax = new Vector2(0.7f, 1f);

                PushViewController(_rightScreenController, true);

                _rightScreenController.CreateSettingsControllers();
            }
            else
            {
                if (_viewControllers.IndexOf(_rightScreenController) < 0)
                {
                    PushViewController(_rightScreenController, true);
                }
            }

            if (_leftScreenController == null)
            {
                _leftScreenController = BeatSaberUI.CreateViewController<NullViewController>();
                _leftScreenController.rectTransform.anchorMin = new Vector2(0.3f, 0f);
                _leftScreenController.rectTransform.anchorMax = new Vector2(0.7f, 1f);

                PushViewController(_leftScreenController, true);
            }
            else
            {
                if (_viewControllers.IndexOf(_leftScreenController) < 0)
                {
                    PushViewController(_leftScreenController, true);
                }
            }

            if (first)
            {
                CreateSettingsControllers();

                List<SimpleSettingsController> list = new List<SimpleSettingsController>(20);
                list.AddRange(GetComponentsInChildren<SimpleSettingsController>(false));
                //list.AddRange(_rightScreenController.GetComponentsInChildren<SimpleSettingsController>());
                _settingControllers = list.ToArray();
                foreach (SimpleSettingsController simpleSettingsController in GetComponentsInChildren<SimpleSettingsController>())
                {
                    simpleSettingsController.Init();
                }
            }
        }

        internal static T CopyListSettingsController<T>(string name, Transform container) where T : ListSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<VolumeSettingsController>().FirstOrDefault();
            volumeSettings.gameObject.SetActive(false);

            var SettingsObject = Instantiate(volumeSettings.gameObject, container);
            SettingsObject.SetActive(false);
            SettingsObject.name = name;

            volumeSettings.gameObject.SetActive(true);

            var volume = SettingsObject.GetComponent<VolumeSettingsController>();
            var newListSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(SimpleSettingsController), typeof(T), SettingsObject);
            DestroyImmediate(volume);

            SettingsObject.GetComponentInChildren<TMP_Text>().text = name;
            //if (autoApply) settingsView.settingControllers.Add(newListSettingsController);

            return newListSettingsController;
        }

        internal static T CopySwitchSettingsController<T>(string name, Transform container) where T : SwitchSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<WindowModeSettingsController>().FirstOrDefault();
            volumeSettings.gameObject.SetActive(false);

            var SettingsObject = Instantiate(volumeSettings.gameObject, container);
            SettingsObject.SetActive(false);
            SettingsObject.name = name;

            volumeSettings.gameObject.SetActive(true);

            var volume = SettingsObject.GetComponent<WindowModeSettingsController>();
            var newSwitchSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(SimpleSettingsController), typeof(T), SettingsObject);
            DestroyImmediate(volume);

            SettingsObject.GetComponentInChildren<TMP_Text>().text = name;
            ///if (autoApply) settingsView.settingControllers.Add(newSwitchSettingsController);

            return newSwitchSettingsController;
        }


        internal void CreateSettingsControllers()
        {
            CopySwitchSettingsController<EnableToggleSettingsController>("Enabled", transform);
            CopyListSettingsController<InputMethodSettingsController>("Input Mode", transform);
            CopyListSettingsController<PressModeSettingsController>("Key Press Method", transform);
            CopyListSettingsController<BindingEditorSelectorSettingsController>("Edit Binding", transform);
        }

        protected void ApplySettings()
        {
            foreach (SimpleSettingsController simpleSettingsController in _settingControllers)
            {
                simpleSettingsController.ApplySettings();
            }
        }

        protected override void LeftAndRightScreenViewControllers(out VRUIViewController leftScreenViewController, out VRUIViewController rightScreenViewController)
        {
            leftScreenViewController = _leftScreenController;
            rightScreenViewController = _rightScreenController;
        }

    }
}

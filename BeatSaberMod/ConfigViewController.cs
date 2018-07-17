using System.Collections.Generic;
using VRUI;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BeatSaberMod
{
    public class ConfigViewController : VRUIViewController
    {
        public VRUIViewController _leftSettings;
        public VRUIViewController _rightSettings;
        public List<SimpleSettingsController> settingControllers = new List<SimpleSettingsController>();
        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (activationType == ActivationType.AddedToHierarchy)
            {
                base.DidActivate(firstActivation, activationType);
                if (firstActivation)
                {
                    SetupButtons();
                    Init();
                }
                VRUIScreen leftScreen = screen.screenSystem.leftScreen;
                VRUIScreen rightScreen = screen.screenSystem.rightScreen;
                leftScreen.SetRootViewController(_leftSettings);
                rightScreen.SetRootViewController(_rightSettings);
            }
        }

        void SetupButtons()
        {
            //DestroyImmediate(transform.Find("OkButton").gameObject);
            Button deleteButton = transform.Find("OkButton").GetComponent<Button>();
            DestroyImmediate(deleteButton.GetComponent<GameEventOnUIButtonClick>());
            deleteButton.onClick = new Button.ButtonClickedEvent();
            deleteButton.onClick.AddListener(DeleteKeybind);
            deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Delete Binding";

            Button addButton = transform.Find("ApplyButton").GetComponent<Button>();
            DestroyImmediate(addButton.GetComponent<GameEventOnUIButtonClick>());
            addButton.onClick = new Button.ButtonClickedEvent();
            addButton.onClick.AddListener(AddKeybind);
            addButton.GetComponentInChildren<TextMeshProUGUI>().text = "Add Binding";

            Button cancelButton = transform.Find("CancelButton").GetComponent<Button>();
            DestroyImmediate(cancelButton.GetComponent<GameEventOnUIButtonClick>());
            cancelButton.onClick = new Button.ButtonClickedEvent();
            cancelButton.onClick.AddListener(CloseButtonPressed);
            cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        }

        public virtual void DeleteKeybind()
        {
            Settings.Bindings.RemoveAt(KeyboardInputObject.Instance.GetSelectedBinding());
            Init(true);
        }

        public virtual void AddKeybind()
        {
            Settings.Bindings.Add(new KeyBinding());
            Init();
        }

        public virtual void Init(bool noapply = false)
        {
            KeyboardInputObject.Instance.SetSelectedBinding(-1, noapply);

            foreach (SimpleSettingsController simpleSettingsController in settingControllers)
            {
                simpleSettingsController.gameObject.SetActive(true);
                simpleSettingsController.Init();
            }
        }

        void ApplySettings()
        {
            foreach (SimpleSettingsController simpleSettingsController in settingControllers)
            {
                simpleSettingsController.ApplySettings();
            }
            Settings.Save();
        }

        public virtual void CloseButtonPressed()
        {
            KeyboardInputObject.Instance.ApplyBindingSettings();
            ApplySettings();
            DismissModalViewController(null, false);
        }
    }

}

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
        protected bool _firstTimeActivated = true;

        public VRUIViewController _leftSettings;
        public VRUIViewController _rightSettings;
        public List<SimpleSettingsController> settingControllers = new List<SimpleSettingsController>();
        protected override void DidActivate()
        {
            base.DidActivate();
            if (_firstTimeActivated)
            {
                _firstTimeActivated = false;
                SetupButtons();
                Init();
            }
            VRUIScreen leftScreen = screen.screenSystem.leftScreen;
            VRUIScreen rightScreen = screen.screenSystem.rightScreen;
            leftScreen.SetRootViewController(_leftSettings);
            rightScreen.SetRootViewController(_rightSettings);
        }

        void SetupButtons()
        {
            DestroyImmediate(transform.Find("OkButton").gameObject);
            DestroyImmediate(transform.Find("ApplyButton").gameObject);
            Button cancelButton = transform.Find("CancelButton").GetComponent<Button>();
            DestroyImmediate(cancelButton.GetComponent<GameEventOnUIButtonClick>());
            cancelButton.onClick = new Button.ButtonClickedEvent();
            cancelButton.onClick.AddListener(CloseButtonPressed);
            cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        }

        public virtual void Init()
        {
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
            ApplySettings();
            DismissModalViewController(null, false);
        }
    }

}

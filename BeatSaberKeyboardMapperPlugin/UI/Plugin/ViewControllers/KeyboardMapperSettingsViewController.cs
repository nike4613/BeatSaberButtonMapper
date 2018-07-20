using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin.ViewControllers
{
    class KeyboardMapperSettingsMasterViewController : VRUINavigationController
    {
        Button _backButton;
        BindingConfigViewController _bindingController;
        SettingsListViewController _settingsController;

        public void Awake()
        {
            _bindingController = BeatSaberUI.CreateViewController<BindingConfigViewController>();
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (_backButton == null)
            {
                _backButton = BeatSaberUI.CreateBackButton(rectTransform);

                _backButton.onClick.AddListener(delegate ()
                {
                    _settingsController.ApplySettings();
                    Settings.Save();
                    DismissModalViewController(null, false);
                });
            }

            if (_settingsController == null)
            {
                _settingsController = BeatSaberUI.CreateViewController<SettingsListViewController>();

                PushViewController(_settingsController, true);
            }
        }

        protected override void LeftAndRightScreenViewControllers(out VRUIViewController leftScreenViewController, out VRUIViewController rightScreenViewController)
        {
            leftScreenViewController = null;
            rightScreenViewController = _bindingController;
        }
    }
}

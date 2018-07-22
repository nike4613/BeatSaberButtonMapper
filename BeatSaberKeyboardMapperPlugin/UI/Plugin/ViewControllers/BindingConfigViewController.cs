using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin.ViewControllers
{
    // for impl look at SettingsViewController
    class BindingConfigViewController : VRUINavigationController
    {
        Button _button;
        Button backButton;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (backButton == null)
            {
                backButton = BeatSaberUI.CreateBackButton(rectTransform);
                backButton.onClick.AddListener(delegate()
                {
                    DismissModalViewController(null, true);
                });
            }
            if (_button == null)
            {
                _button = BeatSaberUI.CreateUIButton(rectTransform, "QuitButton");
                BeatSaberUI.SetButtonText(_button, "Test Button");
;
                _button.onClick.AddListener(delegate ()
                {
                    Logger.log.Debug("test button pressed");
                });
            }
        }
    }
}

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
    class BindingConfigViewController : VRUIViewController
    {
        Button _button;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
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

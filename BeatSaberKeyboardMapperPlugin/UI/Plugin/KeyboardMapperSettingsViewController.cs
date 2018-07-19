using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin
{
    class KeyboardMapperSettingsMasterViewController : VRUINavigationController
    {
        Button _backButton;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (_backButton == null)
            {
                _backButton = BeatSaberUI.CreateBackButton(rectTransform);

                _backButton.onClick.AddListener(delegate ()
                {
                    Settings.Save();
                    DismissModalViewController(null, false);
                });
            }
        }
    }
}

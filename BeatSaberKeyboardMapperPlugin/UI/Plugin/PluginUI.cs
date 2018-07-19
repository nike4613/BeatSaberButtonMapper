using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin
{
    class PluginUI : MonoBehaviour
    {
        static PluginUI _instance;

        private MainMenuViewController _mainMenuViewController;
        private RectTransform _mainMenuRectTransform;

        private KeyboardMapperSettingsMasterViewController _settingsViewController;

        public static void OnLoad()
        {
            if (_instance != null)
            {
                return;
            }
            new GameObject("BeatSaver Plugin").AddComponent<PluginUI>();
        }
        
        public void Awake()
        {
            _instance = this;
        }

        public void Start()
        {
            try
            {
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _mainMenuRectTransform = _mainMenuViewController.transform as RectTransform;

                CreateSettingsButton();
            }
            catch (Exception e)
            {
                Logger.log.Error("EXCEPTION ON AWAKE(TRY CREATE BUTTON): " + e);
                Logger.log.Error(e);
            }
        }

        private void CreateSettingsButton()
        {
            Button _settingsButton = BeatSaberUI.CreateUIButton(_mainMenuRectTransform, "QuitButton");

            try
            {
                (_settingsButton.transform as RectTransform).anchoredPosition = new Vector2(7f, 18f);
                (_settingsButton.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

                BeatSaberUI.SetButtonText(_settingsButton, "Key Bindings");
                //BeatSaberUI.SetButtonIcon(_settingsButton, BeatSaberUI.icons.First(x => x.name == "SettingsIcon"));

                _settingsButton.onClick.AddListener(delegate () {

                    try
                    {
                        if (_settingsViewController == null)
                        {
                            _settingsViewController = BeatSaberUI.CreateViewController<KeyboardMapperSettingsMasterViewController>();
                        }
                        _mainMenuViewController.PresentModalViewController(_settingsViewController, null, false);

                    }
                    catch (Exception e)
                    {
                        Logger.log.Error("EXCETPION IN BUTTON: " + e.Message);
                        Logger.log.Error(e);
                    }

                });

            }
            catch (Exception e)
            {
                Logger.log.Error("EXCEPTION: " + e.Message);
                Logger.log.Error(e);
            }

        }
    }
}

using BeatSaberKeyboardMapperPlugin.UI.Plugin.SettingsControllers;
using BeatSaberKeyboardMapperPlugin.UI.Plugin.ViewControllers;
using BeatSaberKeyboardMapperPlugin.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberKeyboardMapperPlugin.UI.Plugin
{
    class PluginUI : MonoBehaviour
    {
#if false
        internal static PluginUI instance;

        private MainMenuViewController _mainMenuViewController;
        private RectTransform _mainMenuRectTransform;

        private KeyboardMapperSettingsMasterViewController _settingsViewController;

        public static void OnLoad()
        {
            if (instance != null)
            {
                return;
            }
            new GameObject("KeyboardMapper Plugin UI").AddComponent<PluginUI>();
        }
        
        public void Awake()
        {
            instance = this;
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

                _settingsButton.onClick.AddListener(PresentSettings);
            }
            catch (Exception e)
            {
                Logger.log.Error("EXCEPTION: " + e.Message);
                Logger.log.Error(e);
            }

        }

        internal void PresentSettings()
        {
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
        }

#if DEBUG
        internal void HideSettings()
        {
            _settingsViewController.DismissModalViewController(null);
        }
#endif
#else
        public static void Init()
        {
            var submenu = CreateSubMenu("Keyboard Mapper");
            AddToggleSetting<EnabledSettingController>("Enabled", submenu);
        }

        public const int MainScene = 1;

        static MainSettingsTableCell tableCell = null;

        public static Transform CreateSubMenu(string name)
        {
            if (SceneManager.GetActiveScene().buildIndex != MainScene)
            {
                Console.WriteLine("Cannot create settings menu when no in the main scene.");
                return null;
            }

            if (tableCell == null)
            {
                tableCell = Resources.FindObjectsOfTypeAll<MainSettingsTableCell>().FirstOrDefault();
                // Get a refence to the Settings Table cell text in case we want to change fint size, etc
                var text = tableCell.GetPrivateField<TextMeshProUGUI>("_settingsSubMenuText");
            }

            var temp = Resources.FindObjectsOfTypeAll<SettingsViewController>().FirstOrDefault();
            var others = temp.transform.Find("SubSettingsViewControllers").Find("Others");
            var tweakSettingsObject = Instantiate(others.gameObject, others.transform.parent);
            Transform mainContainer = CleanScreen(tweakSettingsObject.transform);

            var tweaksSubMenu = new SettingsSubMenuInfo();
            tweaksSubMenu.SetPrivateField("_menuName", name);
            tweaksSubMenu.SetPrivateField("_controller", tweakSettingsObject.GetComponent<VRUIViewController>());

            var origianlSettingsObject = Resources.FindObjectsOfTypeAll<MainSettingsMenuViewController>().FirstOrDefault();
            var subMenus = origianlSettingsObject.GetPrivateField<SettingsSubMenuInfo[]>("_settingsSubMenuInfos").ToList();
            subMenus.Add(tweaksSubMenu);
            origianlSettingsObject.SetPrivateField("_settingsSubMenuInfos", subMenus.ToArray());

            return mainContainer;
        }

        static Transform CleanScreen(Transform screen)
        {
            var container = screen.Find("SettingsContainer");
            var tempList = container.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
            return container;
        }

        public static void AddListSetting<T>(string name, Transform container) where T : ListSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<VolumeSettingsController>().FirstOrDefault();
            GameObject newSettingsObject = Instantiate(volumeSettings.gameObject, container);
            newSettingsObject.name = name;

            VolumeSettingsController volume = newSettingsObject.GetComponent<VolumeSettingsController>();
            T newListSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(ListSettingsController), typeof(T), newSettingsObject);
            DestroyImmediate(volume);

            newSettingsObject.GetComponentInChildren<TMP_Text>().text = name;
        }

        public static void AddToggleSetting<T>(string name, Transform container) where T : SwitchSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<WindowModeSettingsController>().FirstOrDefault();
            GameObject newSettingsObject = Instantiate(volumeSettings.gameObject, container);
            newSettingsObject.name = name;

            WindowModeSettingsController volume = newSettingsObject.GetComponent<WindowModeSettingsController>();
            T newListSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(SwitchSettingsController), typeof(T), newSettingsObject);
            DestroyImmediate(volume);
            
            newSettingsObject.GetComponentInChildren<TMP_Text>().text = name;
        }
#endif
    }
}

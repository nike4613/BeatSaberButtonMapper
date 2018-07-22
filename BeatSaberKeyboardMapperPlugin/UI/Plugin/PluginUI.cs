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
        public static void Init()
        {
            BeatSaberUI.OnLoad();

            var submenu = CreateSubMenu("Keyboard Mapper");
            AddToggleSetting<EnabledSettingController>("Enabled", submenu);
            AddListSetting<InputModeSettingController>("Input Type", submenu);
            AddListSetting<ControllerModeSettingController>("Controller Type", submenu);

            var uiButton = BeatSaberUI.CreateUIButton(submenu, "CancelButton");
            BeatSaberUI.SetButtonText(uiButton, "Bindings");
            uiButton.onClick.AddListener(PresentSettings);
        }

        static BindingConfigViewController bindingViewController;
        internal static void PresentSettings()
        {
            try
            {
                Logger.log.SuperVerbose("Presenting settings");
                if (bindingViewController == null)
                {
                    Logger.log.SuperVerbose("Creating view controller");
                    bindingViewController = BeatSaberUI.CreateViewController<BindingConfigViewController>();
                }
                Logger.log.SuperVerbose("Presenting view controller");
                mainVC.PresentModalViewController(bindingViewController, null, false);
            }
            catch (Exception e)
            {
                Logger.log.Error("EXCETPION IN BUTTON: " + e.Message);
                Logger.log.Error(e);
            }
        }

        public const int MainScene = 1;

        static MainSettingsTableCell tableCell = null;
        static SettingsViewController mainVC = null;

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
            if (mainVC == null )
                mainVC = Resources.FindObjectsOfTypeAll<SettingsViewController>().FirstOrDefault();
            var others = mainVC.transform.Find("SubSettingsViewControllers").Find("Others");
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
    }
}

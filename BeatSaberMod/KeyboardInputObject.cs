using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRUI;
using IllusionPlugin;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using BeatSaberMod.SettingsControllers;

namespace BeatSaberMod
{
    public class KeyboardInputObject : MonoBehaviour
    {
        public static KeyboardInputObject Instance = null;
        MainMenuViewController _mainMenuViewController = null;
        VRUIViewController _howToPlayViewController = null;
        VRUIViewController _releaseInfoViewController = null;

        ConfigViewController settingsView = null;
        VRUIViewController left = null;
        VRUIViewController right = null;

        //List<string> warningPlugins = new List<string>();

        //bool CameraPlusInstalled = false;

        public static void OnLoad()
        {
            if (Instance != null) return;
            new GameObject("Keyboard Input").AddComponent<KeyboardInputObject>();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);

                Console.WriteLine("KBI started.");
            }
            else
            {
                Destroy(this);
            }
        }

        public void Update()
        {
            
            Input.GetKeyDown((KeyCode)ControllerInput.Vive.LeftMenu);
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex == 1)
            {
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _howToPlayViewController = ReflectionUtil.GetPrivateField<VRUIViewController>(_mainMenuViewController, "_howToPlayViewController");
                _releaseInfoViewController = ReflectionUtil.GetPrivateField<VRUIViewController>(_mainMenuViewController, "_releaseInfoViewController");

                SetupTweakSettings();

                // ~~not enough settings yet
                CreateKeyboardSettingsButton();
            }
        }

        private void Warning_didFinishEvent(SimpleDialogPromptViewController viewController, bool ok)
        {
            viewController.didFinishEvent -= this.Warning_didFinishEvent;
            if (ok)
            {
                viewController.DismissModalViewController(null, false);
            }
            else
            {
                Application.Quit();
            }
        }

        void SetupTweakSettings()
        {
            var origianlSettingsObject = Resources.FindObjectsOfTypeAll<SettingsViewController>().FirstOrDefault();

            var settingsObject = Instantiate(origianlSettingsObject.gameObject, origianlSettingsObject.transform.parent);
            settingsObject.SetActive(false);
            settingsObject.name = "Keyboard Input Settings View Controller";

            var originalSettings = settingsObject.GetComponent<SettingsViewController>();
            settingsView = settingsObject.AddComponent<ConfigViewController>();
            DestroyImmediate(originalSettings);

            left = CopyScreens(origianlSettingsObject, "Left Screen", _howToPlayViewController.transform.parent);
            right = CopyScreens(origianlSettingsObject, "Right Screen", _releaseInfoViewController.transform.parent);

            settingsView._leftSettings = left;
            settingsView._rightSettings = right;

            CleanScreen(settingsView);
            CleanScreen(left);
            CleanScreen(right);

            SetTitle(settingsView, "Input Bindings");
            SetTitle(left, "Input Bindings");
            SetTitle(right, "Input Bindings");

            Transform mainContainer = settingsObject.transform.Find("SettingsContainer");
            Transform leftContainer = left.transform.Find("SettingsContainer");
            Transform rightContainer = right.transform.Find("SettingsContainer");
            SetRectYPos(mainContainer.GetComponent<RectTransform>(), 12);
            SetRectYPos(leftContainer.GetComponent<RectTransform>(), 12);
            SetRectYPos(rightContainer.GetComponent<RectTransform>(), 12);

            CopyListSettingsController<InputMethodSettingsController>("Input Mode", mainContainer);
            CopyListSettingsController<BindingEditorSelectorSettingsController>("Edit Binding", mainContainer);
        }

        void SetRectYPos(RectTransform rect, float y)
        {
            var pos = rect.anchoredPosition;
            pos.y = y;
            rect.anchoredPosition = pos;
        }

        void CleanScreen(VRUIViewController screen)
        {
            var container = screen.transform.Find("SettingsContainer");
            var tempList = container.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        void SetTitle(VRUIViewController screen, string title)
        {
            var titleTransform = screen.transform.Find("Title");
            titleTransform.GetComponent<TextMeshProUGUI>().text = title;
            SetRectYPos(titleTransform.GetComponent<RectTransform>(), -2);
        }

        VRUIViewController CopyScreens(SettingsViewController view, string name, Transform parent)
        {
            var origianlScreen = ReflectionUtil.GetPrivateField<VRUIViewController>(view, "_advancedGraphicsSettingsViewController");
            var tweakScreen = Instantiate(origianlScreen.gameObject, parent);
            tweakScreen.name = name;

            var originalSettings = tweakScreen.GetComponent<VRUIViewController>();
            return originalSettings;
        }

        void CopyListSettingsController<T>(string name, Transform container) where T : ListSettingsController
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
            settingsView.settingControllers.Add(newListSettingsController);
        }

        void CopySwitchSettingsController<T>(string name, Transform container) where T : SwitchSettingsController
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
            settingsView.settingControllers.Add(newSwitchSettingsController);
        }

        private void CreateKeyboardSettingsButton()
        {
            var settingsButton = _mainMenuViewController.transform.Find("SettingsButton").GetComponent<Button>();

            Button btn = Instantiate(settingsButton, settingsButton.transform.parent, false);
            DestroyImmediate(btn.GetComponent<GameEventOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();

            (btn.transform as RectTransform).anchoredPosition = new Vector2(-7f, 18f);
            (btn.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Keybinds";
            btn.onClick.AddListener(ShowTweakSettings);
        }

        public void ShowTweakSettings()
        {
            _mainMenuViewController.PresentModalViewController(settingsView, null, false);
        }
    }
}

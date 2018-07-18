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
using WindowsInput;
using WindowsInput.Native;

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

        Dictionary<KeyCode, VirtualKeyCode> codeConverter = new Dictionary<KeyCode, VirtualKeyCode>() {
            { KeyCode.A, VirtualKeyCode.VK_A },
            { KeyCode.Alpha0, VirtualKeyCode.VK_0 },
            { KeyCode.Alpha1, VirtualKeyCode.VK_1 },
            { KeyCode.Alpha2, VirtualKeyCode.VK_2 },
            { KeyCode.Alpha3, VirtualKeyCode.VK_3 },
            { KeyCode.Alpha4, VirtualKeyCode.VK_4 },
            { KeyCode.Alpha5, VirtualKeyCode.VK_5 },
            { KeyCode.Alpha6, VirtualKeyCode.VK_6 },
            { KeyCode.Alpha7, VirtualKeyCode.VK_7 },
            { KeyCode.Alpha8, VirtualKeyCode.VK_8 },
            { KeyCode.Alpha9, VirtualKeyCode.VK_9 },
            { KeyCode.AltGr, VirtualKeyCode.MENU },
            //{ KeyCode.Ampersand, VirtualKeyCode. },
            //{ KeyCode.Asterisk, VirtualKeyCode. },
            //{ KeyCode.At, VirtualKeyCode. },
            { KeyCode.B, VirtualKeyCode.VK_B },
            { KeyCode.BackQuote, VirtualKeyCode.OEM_3 },
            { KeyCode.Backslash, VirtualKeyCode.OEM_102 },
            { KeyCode.Backspace, VirtualKeyCode.BACK },
            { KeyCode.Break, VirtualKeyCode.PAUSE },
            { KeyCode.C, VirtualKeyCode.VK_C },
            { KeyCode.CapsLock, VirtualKeyCode.CAPITAL },
            //{ KeyCode.Caret, VirtualKeyCode. },
            { KeyCode.Clear, VirtualKeyCode.CLEAR },
            { KeyCode.Colon, VirtualKeyCode.OEM_1 },
            { KeyCode.Comma, VirtualKeyCode.OEM_COMMA },
            { KeyCode.D, VirtualKeyCode.VK_D },
            { KeyCode.Delete, VirtualKeyCode.DELETE },
            //{ KeyCode.Dollar, VirtualKeyCode. },
            { KeyCode.DoubleQuote, VirtualKeyCode.OEM_7 },
            { KeyCode.DownArrow, VirtualKeyCode.DOWN },
            { KeyCode.E, VirtualKeyCode.VK_E },
            { KeyCode.End, VirtualKeyCode.END },
            //{ KeyCode.Equals, VirtualKeyCode. },
            { KeyCode.Escape, VirtualKeyCode.ESCAPE },
            //{ KeyCode.Exclaim, VirtualKeyCode. },
            { KeyCode.F, VirtualKeyCode.VK_F },
            { KeyCode.F1, VirtualKeyCode.F1 },
            { KeyCode.F2, VirtualKeyCode.F2 },
            { KeyCode.F3, VirtualKeyCode.F3 },
            { KeyCode.F4, VirtualKeyCode.F4 },
            { KeyCode.F5, VirtualKeyCode.F5 },
            { KeyCode.F6, VirtualKeyCode.F6 },
            { KeyCode.F7, VirtualKeyCode.F7 },
            { KeyCode.F8, VirtualKeyCode.F8 },
            { KeyCode.F9, VirtualKeyCode.F9 },
            { KeyCode.F10, VirtualKeyCode.F10 },
            { KeyCode.F11, VirtualKeyCode.F11 },
            { KeyCode.F12, VirtualKeyCode.F12 },
            { KeyCode.F13, VirtualKeyCode.F13 },
            { KeyCode.F14, VirtualKeyCode.F14 },
            { KeyCode.F15, VirtualKeyCode.F15 },
            { KeyCode.G, VirtualKeyCode.VK_G },
            //{ KeyCode.Greater, VirtualKeyCode. },
            { KeyCode.H, VirtualKeyCode.VK_H },
            //{ KeyCode.Hash, VirtualKeyCode. },
            { KeyCode.Help, VirtualKeyCode.HELP },
            { KeyCode.Home, VirtualKeyCode.HOME },
            { KeyCode.I, VirtualKeyCode.VK_I },
            { KeyCode.Insert, VirtualKeyCode.INSERT },
            { KeyCode.J, VirtualKeyCode.VK_J },
            { KeyCode.K, VirtualKeyCode.VK_K},
            { KeyCode.Keypad0, VirtualKeyCode.NUMPAD0 },
            { KeyCode.Keypad1, VirtualKeyCode.NUMPAD1 },
            { KeyCode.Keypad2, VirtualKeyCode.NUMPAD2 },
            { KeyCode.Keypad3, VirtualKeyCode.NUMPAD3 },
            { KeyCode.Keypad4, VirtualKeyCode.NUMPAD4 },
            { KeyCode.Keypad5, VirtualKeyCode.NUMPAD5 },
            { KeyCode.Keypad6, VirtualKeyCode.NUMPAD6 },
            { KeyCode.Keypad7, VirtualKeyCode.NUMPAD7 },
            { KeyCode.Keypad8, VirtualKeyCode.NUMPAD8 },
            { KeyCode.Keypad9, VirtualKeyCode.NUMPAD9 },
            { KeyCode.KeypadDivide, VirtualKeyCode.DIVIDE },
            { KeyCode.KeypadEnter, VirtualKeyCode.RETURN },
            //{ KeyCode.KeypadEquals, VirtualKeyCode. },
            { KeyCode.KeypadMinus, VirtualKeyCode.OEM_MINUS },
            { KeyCode.KeypadMultiply, VirtualKeyCode.MULTIPLY },
            { KeyCode.KeypadPeriod, VirtualKeyCode.OEM_PERIOD },
            { KeyCode.KeypadPlus, VirtualKeyCode.OEM_PLUS },
            { KeyCode.L, VirtualKeyCode.VK_L },
            { KeyCode.LeftAlt, VirtualKeyCode.LMENU },
            { KeyCode.LeftApple, VirtualKeyCode.LCONTROL },
            { KeyCode.LeftArrow, VirtualKeyCode.LEFT },
            { KeyCode.LeftBracket, VirtualKeyCode.OEM_4 },
            { KeyCode.LeftCommand, VirtualKeyCode.LCONTROL },
            { KeyCode.LeftControl, VirtualKeyCode.LCONTROL },
            //{ KeyCode.LeftParen, VirtualKeyCode. },
            { KeyCode.LeftShift, VirtualKeyCode.LSHIFT },
            { KeyCode.LeftWindows, VirtualKeyCode.LWIN },
            //{ KeyCode.Less, VirtualKeyCode. },
            { KeyCode.M, VirtualKeyCode.VK_M },
            { KeyCode.Menu, VirtualKeyCode.MENU },
            { KeyCode.Minus, VirtualKeyCode.OEM_MINUS },
            { KeyCode.Mouse0, VirtualKeyCode.LBUTTON },
            { KeyCode.Mouse1, VirtualKeyCode.RBUTTON },
            { KeyCode.Mouse2, VirtualKeyCode.MBUTTON },
            { KeyCode.Mouse3, VirtualKeyCode.XBUTTON1 },
            { KeyCode.Mouse4, VirtualKeyCode.XBUTTON2 },
            //{ KeyCode.Mouse5, VirtualKeyCode. },
            //{ KeyCode.Mouse6, VirtualKeyCode. },
            { KeyCode.N, VirtualKeyCode.VK_N },
            //{ KeyCode.None, VirtualKeyCode. },
            { KeyCode.Numlock, VirtualKeyCode.NUMLOCK },
            { KeyCode.O, VirtualKeyCode.VK_O },
            { KeyCode.P, VirtualKeyCode.VK_P },
            { KeyCode.PageDown, VirtualKeyCode.NEXT },
            { KeyCode.PageUp, VirtualKeyCode.PRIOR },
            { KeyCode.Pause, VirtualKeyCode.PAUSE },
            { KeyCode.Period, VirtualKeyCode.OEM_PERIOD },
            { KeyCode.Plus, VirtualKeyCode.OEM_PLUS },
            { KeyCode.Print, VirtualKeyCode.PRINT },
            { KeyCode.Q, VirtualKeyCode.VK_Q },
            { KeyCode.Question, VirtualKeyCode.OEM_2 },
            { KeyCode.Quote, VirtualKeyCode.OEM_7 },
            { KeyCode.R, VirtualKeyCode.VK_R },
            { KeyCode.Return, VirtualKeyCode.RETURN },
            { KeyCode.RightAlt, VirtualKeyCode.RMENU },
            { KeyCode.RightApple, VirtualKeyCode.RCONTROL },
            { KeyCode.RightArrow, VirtualKeyCode.RIGHT },
            { KeyCode.RightBracket, VirtualKeyCode.OEM_6 },
            { KeyCode.RightCommand, VirtualKeyCode.RCONTROL },
            { KeyCode.RightControl, VirtualKeyCode.RCONTROL },
            //{ KeyCode.RightParen, VirtualKeyCode. },
            { KeyCode.RightShift, VirtualKeyCode.RSHIFT },
            { KeyCode.RightWindows, VirtualKeyCode.RWIN },
            { KeyCode.S, VirtualKeyCode.VK_S },
            { KeyCode.ScrollLock, VirtualKeyCode.SCROLL },
            { KeyCode.Semicolon, VirtualKeyCode.OEM_1 },
            { KeyCode.Slash, VirtualKeyCode.OEM_2 },
            { KeyCode.Space, VirtualKeyCode.SPACE },
            //{ KeyCode.SysReq, VirtualKeyCode. },
            { KeyCode.T, VirtualKeyCode.VK_T },
            { KeyCode.Tab, VirtualKeyCode.TAB },
            { KeyCode.U, VirtualKeyCode.VK_U },
            //{ KeyCode.Underscore, VirtualKeyCode. },
            { KeyCode.UpArrow, VirtualKeyCode.UP },
            { KeyCode.V, VirtualKeyCode.VK_V },
            { KeyCode.W, VirtualKeyCode.VK_W },
            { KeyCode.X, VirtualKeyCode.VK_X },
            { KeyCode.Y, VirtualKeyCode.VK_Y },
            { KeyCode.Z, VirtualKeyCode.VK_Z },
        };

        InputSimulator simulator = new InputSimulator();
        public void Update()
        {
            if ((Settings.InputMode & InputMode.System) != 0)
            {
                //Console.WriteLine("checking for input");
                foreach (var binding in Settings.Bindings)
                {
                    if (Input.GetKeyDown(binding.SourceKey))
                        simulator.Keyboard.KeyDown(codeConverter[binding.DestKey]);
                    if (Input.GetKeyUp(binding.SourceKey))
                        simulator.Keyboard.KeyUp(codeConverter[binding.DestKey]);
                }
                //Console.WriteLine("input checked");
            }
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

        ControllerButtonSelectorController cntrlBtnSel;
        KeyboardButtonSelectorController keybBtnSel;
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
            SetTitle(left, "");
            SetTitle(right, "Edit Binding");

            Transform mainContainer = settingsObject.transform.Find("SettingsContainer");
            Transform leftContainer = left.transform.Find("SettingsContainer");
            Transform rightContainer = right.transform.Find("SettingsContainer");
            SetRectYPos(mainContainer.GetComponent<RectTransform>(), 12);
            SetRectYPos(leftContainer.GetComponent<RectTransform>(), 12);
            SetRectYPos(rightContainer.GetComponent<RectTransform>(), 12);

            CopySwitchSettingsController<EnableToggleSettingsController>("Enabled", mainContainer);
            CopyListSettingsController<InputMethodSettingsController>("Input Mode", mainContainer);
            CopyListSettingsController<PressModeSettingsController>("Key Press Method", mainContainer);
            CopyListSettingsController<BindingEditorSelectorSettingsController>("Edit Binding", mainContainer);

            Transform menuParent = rightContainer;//sideMenuParentObject.transform;

            cntrlBtnSel = CopyListSettingsController<ControllerButtonSelectorController>("Map", menuParent, false);
            CopyListSettingsController<EmptyListSettingsController>("", menuParent); // spacer
            CopyListSettingsController<EmptyListSettingsController>("", menuParent); // spacer
            keybBtnSel = CopyListSettingsController<KeyboardButtonSelectorController>("to", menuParent, false);

            SetSelectedBinding(-1);
        }

        int curSelectedBinding = -1;
        public void ApplyBindingSettings()
        {
            if (curSelectedBinding != -1)
            {
                cntrlBtnSel.ApplySettings();
                keybBtnSel.ApplySettings();
            }
        }
        public int GetSelectedBinding() => curSelectedBinding;
        public void SetSelectedBinding(int index, bool noapply = false)
        {
            Console.WriteLine($"Selecting binding {index}");

            if (!noapply && index != curSelectedBinding)
                ApplyBindingSettings();

            Console.WriteLine("Applied old settings");

            if (index == -1)
            {
                cntrlBtnSel.gameObject.SetActive(false);
                var rendr = cntrlBtnSel.gameObject.GetComponent<Renderer>();
                if (rendr != null) rendr.enabled = false;
                keybBtnSel.gameObject.SetActive(false);
                rendr = keybBtnSel.gameObject.GetComponent<Renderer>();
                if (rendr != null) rendr.enabled = false;

                Console.WriteLine("Disabled parent");
            }
            else
            {
                cntrlBtnSel.gameObject.SetActive(true);
                var rendr = cntrlBtnSel.gameObject.GetComponent<MeshRenderer>();
                if (rendr != null) rendr.enabled = true;
                keybBtnSel.gameObject.SetActive(true);
                rendr = keybBtnSel.gameObject.GetComponent<MeshRenderer>();
                if (rendr != null) rendr.enabled = true;

                Console.WriteLine("Enabled parent");
                Console.WriteLine($"{index} != {curSelectedBinding} : {index != curSelectedBinding}");
            }

            if (index != curSelectedBinding)
            {
                cntrlBtnSel.SelectedIndex = index;
                cntrlBtnSel.Init();
                Console.WriteLine("Inited cntrlBtnSel");
                keybBtnSel.SelectedIndex = index;
                keybBtnSel.Init();
                Console.WriteLine("Inited keybBtnSel");
            }

            curSelectedBinding = index;
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

        T CopyListSettingsController<T>(string name, Transform container, bool autoApply = true) where T : ListSettingsController
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
            if (autoApply) settingsView.settingControllers.Add(newListSettingsController);

            return newListSettingsController;
        }

        T CopySwitchSettingsController<T>(string name, Transform container, bool autoApply = true) where T : SwitchSettingsController
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
            if (autoApply) settingsView.settingControllers.Add(newSwitchSettingsController);

            return newSwitchSettingsController;
        }

        private void CreateKeyboardSettingsButton()
        {
            var settingsButton = _mainMenuViewController.transform.Find("SettingsButton").GetComponent<Button>();

            Button btn = Instantiate(settingsButton, settingsButton.transform.parent, false);
            DestroyImmediate(btn.GetComponent<GameEventOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();

            (btn.transform as RectTransform).anchoredPosition = new Vector2(-7f, 30f);
            (btn.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Keybinds";
            btn.onClick.AddListener(ShowSettings);
        }

        public void ShowSettings()
        {
            Console.WriteLine("Showing Settings");
            settingsView.Init();
            _mainMenuViewController.PresentModalViewController(settingsView, null, false);
        }
    }
}

using BeatSaberKeyboardMapperPlugin.Utilities;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatSaberKeyboardMapperPlugin
{
    [Serializable]
    public enum ControllerMode
    {
        Oculus,
        Vive,
        WinMR
    }

    [Flags]
    [Serializable]
    public enum InputMode
    {
        Default = 1, // Patch Unity's Input class
        System = 2, // Send keypresses to the OS
        Both = Default | System // Send presses to both (could cause problems)
    }

    [Serializable]
    public class KeyBinding
    {
        /// <summary>
        /// Stores the key to press
        /// </summary>
        public KeyCode SourceKey;
        /// <summary>
        /// Stores the key that will be pressed
        /// </summary>
        public KeyCode DestKey;

        public override string ToString()
        {
            return $"{SourceKey.ToNiceName()} => {DestKey.ToNiceName()}";
        }
    }

    [Serializable]
    public enum ControllerAxis
    {
        TriggerLeftHand,
        TriggerRightHand,
        VerticalLeftHand,
        VerticalRightHand,
        HorizontalLeftHand,
        HorizontalRightHand
    }

    [Serializable]
    public class ControllerAxisBinding
    {
        public KeyCode SourceKey;

        public ControllerAxis Axis;
        public float? OffValue;
        public float OnValue;

        public override string ToString()
        {
            return $"{SourceKey.ToNiceName()} => {Axis.ToNiceName()}: {(OffValue == null ? "?" : OffValue.ToString())} - {OnValue}";
        }
    }

    [Serializable]
    public class Settings
    {
        static Settings instance = new Settings();

        private bool ready = false;

        public bool enabled = true;
        public static bool Enabled { get => instance.enabled; set => instance.enabled = value; }

        // Controller Type
        public ControllerMode controllerMode = default(ControllerMode);
        public static ControllerMode ControllerMode { get => instance.controllerMode; set => instance.controllerMode = value; }

        public InputMode inputMode = InputMode.Default;
        public static InputMode InputMode { get => instance.inputMode; set => instance.inputMode = value; }

        public List<KeyBinding> bindings = new List<KeyBinding>();
        public static List<KeyBinding> Bindings { get => instance.bindings; }
        public List<ControllerAxisBinding> axisBindings = new List<ControllerAxisBinding>();
        public static List<ControllerAxisBinding> AxisBindings { get => instance.axisBindings; }

        public Settings()
        {

        }

        public static string SettingsPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "keybinds.json");
        }

        public static void Load()
        {

            string filePath = SettingsPath();
            if (File.Exists(filePath))
            {
                Func<KeyValuePair<string, JSONNode>, bool> KeySel(string n) => (KeyValuePair<string, JSONNode> p) => p.Key == n;
                try
                {
                    instance = new Settings();

                    var str = File.ReadAllText(filePath);
                    var json = JSON.Parse(str).AsObject;

                    instance.enabled = json["enabled"].AsBool;
                    instance.controllerMode = (ControllerMode)Enum.Parse(typeof(ControllerMode), json["controller"].Value);
                    instance.inputMode = (InputMode)Enum.Parse(typeof(InputMode), json["input"].Value);

                    foreach (var val in json["keybinds"].AsArray.Children)
                    {
                        var obj = val.AsObject;
                        var kb = new KeyBinding
                        {
                            SourceKey = (KeyCode)Enum.Parse(typeof(KeyCode), obj["source"].Value),
                            DestKey = (KeyCode)Enum.Parse(typeof(KeyCode), obj["dest"].Value)
                        };
                        instance.bindings.Add(kb);
                    }
                    foreach (var val in json["axisbinds"].AsArray.Children)
                    {
                        var obj = val.AsObject;
                        var kb = new ControllerAxisBinding
                        {
                            SourceKey = (KeyCode)Enum.Parse(typeof(KeyCode), obj["source"].Value),
                            Axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), obj["axis"].Value),
                            OnValue = (float)obj["on"].AsDouble
                        };
                        if (obj.Linq.Any(KeySel("off"))) // has key
                            kb.OffValue = (float)obj["off"].AsDouble;
                        else
                            kb.OffValue = null;
                        instance.axisBindings.Add(kb);
                    }

                    instance.ready = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            if (!instance.ready)
            {
                instance = new Settings()
                {
                    ready = true
                };
            }
        }

        public static void Save()
        {
            var root = new JSONObject();
            root["enabled"] = instance.enabled;
            root["controller"] = instance.controllerMode.ToString();
            root["input"] = instance.inputMode.ToString();

            var bindArr = (root["keybinds"] = new JSONArray()).AsArray;
            foreach (var val in instance.bindings)
            {
                var obj = new JSONObject();
                obj["source"] = val.SourceKey.ToString();
                obj["dest"] = val.SourceKey.ToString();
                bindArr.Add(obj);
            }

            var axisArr = (root["axisbinds"] = new JSONArray()).AsArray;
            foreach (var val in instance.axisBindings)
            {
                var obj = new JSONObject();
                obj["source"] = val.SourceKey.ToString();
                obj["axis"] = val.Axis.ToString();
                obj["on"] = val.OnValue;
                if (val.OffValue != null)
                    obj["off"] = val.OffValue.Value;
                axisArr.Add(obj);
            }

            File.WriteAllText(SettingsPath(), root.ToString(2));
        }
    }
}

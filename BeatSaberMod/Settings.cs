using BeatSaberMod.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BeatSaberMod
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
            return Path.Combine(Environment.CurrentDirectory, "keybinds.cfg");
        }

        public static void Load()
        {

            string filePath = SettingsPath();
            if (File.Exists(filePath))
            {
                var fstream = File.OpenRead(SettingsPath());
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                try
                {
                    instance = (Settings)serializer.Deserialize(fstream);
                    instance.ready = true;
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
                fstream.Close();
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
            var fstream = File.Open(SettingsPath(), FileMode.Create, FileAccess.Write);
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(fstream, instance);
            fstream.Close();
        }
    }
}

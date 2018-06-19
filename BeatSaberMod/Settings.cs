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
    public class Settings
    {
        static Settings instance = null;

        public bool enabled = true;
        public static bool Enabled { get => instance.enabled; set => instance.enabled = value; }

        // Controller Type
        public ControllerMode controllerMode = default(ControllerMode);
        public static ControllerMode ControllerMode { get => instance.controllerMode; set => instance.controllerMode = value; }
        
        public List<KeyBinding> bindings = new List<KeyBinding>();
        public static List<KeyBinding> Bindings { get => instance.bindings; }

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
                instance = (Settings)serializer.Deserialize(fstream);
                fstream.Close();
            }
            else
            {
                instance = new Settings();
            }
        }

        public static void Save()
        {
            var fstream = File.OpenWrite(SettingsPath());
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(fstream, instance);
            fstream.Close();
        }
    }
}

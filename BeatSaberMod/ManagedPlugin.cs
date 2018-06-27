using BeatSaberMod.HarmonyPatches;
using BeatSaberModManager.Meta;
using BeatSaberModManager.Plugin;
using BeatSaberModManager.Utilities.Logging;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMod
{
    [BeatSaberPlugin]
    class ManagedPlugin : IBeatSaberPlugin
    {
        public static LoggerBase Log;
        private static HarmonyInstance harmony;

        public void Init(LoggerBase logger)
        {
            Log = logger;

            IPAPlugin.shouldInit = false;

            harmony = HarmonyInstance.Create("com.cirr.beatsaber.keyboardinput");
            UnityEngineInputPatch.Patch(harmony);

            Log.Debug("UnityEngine.Input patched");

            Settings.Load();

            Log.SuperVerbose("Settings loaded");
        }

        public void OnApplicationQuit()
        {
            Settings.Save();
        }

        public void OnApplicationStart()
        {
            foreach (var binding in Settings.Bindings)
                Console.WriteLine(binding);

            KeyboardInputObject.OnLoad();
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
        }
    }
}

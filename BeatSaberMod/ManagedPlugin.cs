#if MANAGED
using BeatSaberMod.HarmonyPatches;
using BeatSaberMod.IPA.UI;
using BeatSaberModManager.Meta;
using BeatSaberModManager.Plugin;
using BeatSaberModManager.Utilities.Logging;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatSaberMod
{
    [BeatSaberPlugin(name: "KeyboardInput")]
    class ManagedPlugin : IBeatSaberPlugin
    {
        private static HarmonyInstance harmony;

        public Version Version => new Version(0, 0, 3);

        public void Init(LoggerBase logger)
        {
            Logging._log = logger;

            //IPAPlugin.shouldInit = false;

            harmony = HarmonyInstance.Create("com.cirr.beatsaber.keyboardinput");
            UnityEngineInputPatch.Patch(harmony);

            Logging.log.Debug("UnityEngine.Input patched");

            Settings.Load();

            Logging.log.SuperVerbose("Settings loaded");
        }

        public void OnApplicationQuit()
        {
            Settings.Save();
        }

        public void OnApplicationStart()
        {
            foreach (var binding in Settings.Bindings)
                Logging.log.Debug(binding.ToString());
            if (Settings.AxisBindings.Count == 0)
            {
                Logging.log.Debug("No axis bindings avaliable");
                Settings.AxisBindings.Add(new ControllerAxisBinding
                {
                    SourceKey = KeyCode.Return,
                    Axis = ControllerAxis.TriggerRightHand,
                    OnValue = 1.0f,
                    OffValue = null
                });
            }
            foreach (var axisBind in Settings.AxisBindings)
                Logging.log.Debug(axisBind.ToString());

            Settings.Save();

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
            if (level == 1)
            { // main menu
                BeatSaberUI.OnLoad();
                PluginUI.OnLoad();
            }
        }

        public void OnUpdate()
        {
        }
    }
}

#endif
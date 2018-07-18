using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using System.Reflection;
using Harmony;
using BeatSaberMod.HarmonyPatches;

namespace BeatSaberMod
{
    public class IPAPlugin : IPlugin
    {
        private static bool init = false;
        internal static bool shouldInit = true;
        private static HarmonyInstance harmony;
        public IPAPlugin()
        {
            if (!shouldInit) throw new Exception("Tried to load IPA plugin when mod manager is present!");
            if (init) return;
            init = true;

            harmony = HarmonyInstance.Create("com.cirr.beatsaber.keyboardinput");
            UnityEngineInputPatch.Patch(harmony);

            Console.WriteLine("Input Patched");
        }

        private bool _init = false;

        public string Name => "Keyboard Input Plugin";
        public string Version => "0.0.3";
        public void OnApplicationStart()
        {
            if (_init) return;
            _init = true;

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            Settings.Load();

            foreach (var binding in Settings.Bindings)
                Console.WriteLine(binding);
            if (Settings.AxisBindings.Count == 0)
            {
                Console.WriteLine("No axis bindings avaliable");
                Settings.AxisBindings.Add(new ControllerAxisBinding
                {
                    SourceKey = KeyCode.Return,
                    Axis = ControllerAxis.TriggerRightHand,
                    OnValue = 1.0f,
                    OffValue = null
                });
            }
            foreach (var axisBind in Settings.AxisBindings)
                Console.WriteLine(axisBind);

            Settings.Save();

            KeyboardInputObject.OnLoad();
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

            Settings.Save();
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}

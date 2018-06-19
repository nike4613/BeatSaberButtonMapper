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
    public class KeyboardInputPlugin : IPlugin
    {
        private static bool init = false;
        private static HarmonyInstance harmony;
        static KeyboardInputPlugin()
        {
            if (init) return;
            init = true;

            harmony = HarmonyInstance.Create("com.cirr.beatsaber.keyboardinput");
            UnityEngineInputPatch.Patch(harmony);

            Console.WriteLine("Input Patched");
        }

        private bool _init = false;

        public string Name => "Keyboard Input Plugin";
        public string Version => "0.0.1";
        public void OnApplicationStart()
        {
            if (_init) return;
            _init = true;

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            Settings.Load();

            if (Settings.Bindings.Count == 0)
            {
                Settings.Bindings.Add(new KeyBinding()
                {
                    SourceKey = (KeyCode)ControllerInput.Vive.RightTrackpadPress,
                    DestKey = KeyCode.P
                });
            }

            foreach (var binding in Settings.Bindings)
                Console.WriteLine(binding);

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

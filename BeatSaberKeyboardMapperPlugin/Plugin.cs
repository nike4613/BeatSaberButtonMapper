using BeatSaberKeyboardMapperPlugin.Harmony;
using BeatSaberKeyboardMapperPlugin.UI;
#if MANAGED
using BeatSaberModManager.Meta;
using BeatSaberModManager.Plugin;
using BeatSaberModManager.Utilities.Logging;
#endif
using Harmony;
using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatSaberKeyboardMapperPlugin
{
#if MANAGED
    [BeatSaberPlugin(name: "Keyboard Mapper")]
    public class Plugin : IBeatSaberPlugin
#else
    public class Plugin : IPlugin
#endif
    {
#if !MANAGED
        public Plugin()
        {
            EarlyInit();
        }
#endif

        private Version _version = new Version(0, 1, 0);

#if MANAGED
        public void Init(LoggerBase log)
        {
            Logger._mlog = log;
            EarlyInit();
        }

        public Version Version => _version;
#else
        public string Name => "Keyboard Mapper";

        public string Version => _version.ToString();
#endif

        public HarmonyInstance harmony;

        public void EarlyInit()
        {
            Settings.Load();

            harmony = HarmonyInstance.Create("com.cirr.beatsaber.keyboardinput");
            BeatSaberPatches.Patch(harmony);
            Logger.log.Debug("Patched Unity");
            
        }

        public void OnApplicationQuit()
        {
            Settings.Save();
        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("Plugin OnApplicationStart");

            foreach (var binding in Settings.Bindings)
                Logger.log.Debug(binding.ToString());
            if (Settings.AxisBindings.Count == 0)
            {
                Logger.log.Debug("No axis bindings avaliable");
                Settings.AxisBindings.Add(new ControllerAxisBinding
                {
                    SourceKey = KeyCode.Return,
                    Axis = ControllerAxis.TriggerRightHand,
                    OnValue = 1.0f,
                    OffValue = null
                });
            }
            foreach (var axisBind in Settings.AxisBindings)
                Logger.log.Debug(axisBind.ToString());

            Settings.Save();
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
            {
                BeatSaberUI.OnLoad(); // init the UI lib
                UI.Plugin.PluginUI.OnLoad(); // init our UI
            }
        }

        public void OnUpdate()
        {

        }
    }
}

using BeatSaberKeyboardMapperPlugin.Harmony;
using BeatSaberKeyboardMapperPlugin.UI;
using BeatSaberKeyboardMapperPlugin.UI.Plugin;
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
#if DEBUG
            Logger.log.Info("Built with DEBUG symbol");
            Logger.log.Info("Press '\\' to open the UI");
#endif

            foreach (var binding in Settings.Bindings)
                Logger.log.Debug(binding.ToString());
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
                PluginUI.Init();
            }
        }
        
        public void OnUpdate()
        {

        }
    }
}

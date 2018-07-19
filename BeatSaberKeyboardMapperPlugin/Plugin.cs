using BeatSaberModManager.Meta;
using BeatSaberModManager.Plugin;
using BeatSaberModManager.Utilities.Logging;
using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberKeyboardMapperPlugin
{
#if MANAGED
    [BeatSaberPlugin(name: "Keyboard Mapper")]
    public class Plugin : IBeatSaberPlugin
#else
    public class Plugin : IPlugin
#endif
    {
        public Plugin()
        {

        }

#if MANAGED
        public void Init(LoggerBase log)
        {
            Logger._mlog = log;
        }

        public Version Version => new Version(0, 1, 0);
#else
        public string Name => "Keyboard Mapper";

        public string Version => "0.1.0";
#endif

        public void OnApplicationQuit()
        {

        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("Plugin OnApplicationStart");
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

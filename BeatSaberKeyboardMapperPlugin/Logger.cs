#if MANAGED
using BeatSaberModManager.Utilities.Logging;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberKeyboardMapperPlugin
{
    internal enum Level : byte
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        Critical = 16,
        SuperVerbose = 32
    }

    // Usage: Logger.log.<type>(<message>)
    internal class Logger
    {
        public static LoggerImpl log = new LoggerImpl();
#if MANAGED
        public static LoggerBase _mlog;
#endif

        public class LoggerImpl
        {
            public void Log(Level level, string message)
            {
#if MANAGED
                _mlog.Log((LoggerBase.Level)level, message);
#else
                Console.WriteLine($"{level}: {message}", args);
#endif
            }
            public void Log(Level level, Exception exeption) => Log(level, exeption.ToString());
            public void SuperVerbose(string message) => Log(Level.SuperVerbose, message);
            public void SuperVerbose(Exception e) => Log(Level.SuperVerbose, e);
            public void Debug(string message) => Log(Level.Debug, message);
            public void Debug(Exception e) => Log(Level.Debug, e);
            public void Info(string message) => Log(Level.Info, message);
            public void Info(Exception e) => Log(Level.Info, e);
            public void Warn(string message) => Log(Level.Warning, message);
            public void Warn(Exception e) => Log(Level.Warning, e);
            public void Error(string message) => Log(Level.Error, message);
            public void Error(Exception e) => Log(Level.Error, e);
            public void Critical(string message) => Log(Level.Critical, message);
            public void Critical(Exception e) => Log(Level.Critical, e);
        }
    }
}

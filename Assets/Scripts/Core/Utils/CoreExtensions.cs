using Core.DependencyManager;
using Core.EventChannel;
using Core.Utils.Logger;
using LogType = Core.Utils.Logger.LogType;

namespace Core.Utils
{
    public static class CoreExtensions
    {
        public static void Log(this object anyObject, string msg, LogType type = LogType.Other)
        {
            DI.Get<Logger.Logger>().Log(msg, type);
        }
        
        public static void LogError(this object anyObject, string msg, LogType type = LogType.Other)
        {
            DI.Get<Logger.Logger>().Log(msg, type, LogLevel.Error);
        }
        
        public static void LogWarning(this object anyObject, string msg, LogType type = LogType.Other)
        {
            DI.Get<Logger.Logger>().Log(msg, type, LogLevel.Warning);
        }

        public static IEventChannel GetChannel(this object anyObject)
        {
            var result = DI.Get<IEventChannel>();

            if (result == null)
            {
                DI.Add<IEventChannel>(new EventChannel.EventChannel());
                result = DI.Get<IEventChannel>();
            }

            return result;
        }

        public static IEventChannel<TArg> GetChannel<TArg>(this object anyObject)
        {
            var result = DI.Get<IEventChannel<TArg>>();

            if (result == null)
            {
                DI.Add<IEventChannel<TArg>>(new EventChannel<TArg>());
                result = DI.Get<IEventChannel<TArg>>();
            }

            return result;
        }

        public static IEventChannel<TArg1, TArg2> GetChannel<TArg1, TArg2>(this object anyObject)
        {
            var result = DI.Get<IEventChannel<TArg1, TArg2>>();

            if (result == null)
            {
                DI.Add<IEventChannel<TArg1, TArg2>>(new EventChannel<TArg1, TArg2>());
                result = DI.Get<IEventChannel<TArg1, TArg2>>();
            }

            return result;
        }

        public static IEventChannel<TArg1, TArg2, TArg3> GetChannel<TArg1, TArg2, TArg3>(this object anyObject)
        {
            var result = DI.Get<IEventChannel<TArg1, TArg2, TArg3>>();

            if (result == null)
            {
                DI.Add<IEventChannel<TArg1, TArg2, TArg3>>(
                    new EventChannel<TArg1, TArg2, TArg3>());
                result = DI.Get<IEventChannel<TArg1, TArg2, TArg3>>();
            }

            return result;
        }

        public static IEventChannel<TArg1, TArg2, TArg3, TArg4> GetChannel<TArg1, TArg2, TArg3, TArg4>(
            this object anyObject)
        {
            var result = DI.Get<IEventChannel<TArg1, TArg2, TArg3, TArg4>>();

            if (result == null)
            {
                DI.Add<IEventChannel<TArg1, TArg2, TArg3, TArg4>>(
                    new EventChannel<TArg1, TArg2, TArg3, TArg4>());
                result = DI.Get<IEventChannel<TArg1, TArg2, TArg3, TArg4>>();
            }

            return result;
        }

    }
}
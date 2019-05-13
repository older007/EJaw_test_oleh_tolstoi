using System;

namespace Core.EventChannel
{
    public interface IEventChannel
    {
        void RaiseEvent(string topic);
        void Subscribe(string topic, Action callback);
        void UnSubscribe(string topic, Action callback);
    }

    public interface IEventChannel<TArg>
    {
        void RaiseEvent(string topic, TArg arg);
        void Subscribe(string topic, Action<TArg> callback);
        void UnSubscribe(string topic, Action<TArg> callback);
    }

    public interface IEventChannel<TArg1, TArg2>
    {
        void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2);
        void Subscribe(string topic, Action<TArg1, TArg2> callback);
        void UnSubscribe(string topic, Action<TArg1, TArg2> callback);
    }

    public interface IEventChannel<TArg1, TArg2, TArg3>
    {
        void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2, TArg3 arg3);
        void Subscribe(string topic, Action<TArg1, TArg2, TArg3> callback);
        void UnSubscribe(string topic, Action<TArg1, TArg2, TArg3> callback);
    }

    public interface IEventChannel<TArg1, TArg2, TArg3, TArg4>
    {
        void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
        void Subscribe(string topic, Action<TArg1, TArg2, TArg3, TArg4> callback);
        void UnSubscribe(string topic, Action<TArg1, TArg2, TArg3, TArg4> callback);
    }
}
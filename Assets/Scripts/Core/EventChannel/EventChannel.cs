using System;
using System.Collections.Generic;
using Core.Utils;
using Core.Utils.Logger;

namespace Core.EventChannel
{
    public class EventChannel : IEventChannel
    {
        private readonly Dictionary<string, List<Action>> topics =
            new Dictionary<string, List<Action>>();

        private readonly Dictionary<string, List<Action>> topicsToAdd =
            new Dictionary<string, List<Action>>();

        private readonly Dictionary<string, List<Action>> topicsToAddCache =
            new Dictionary<string, List<Action>>();

        private readonly Dictionary<string, List<Action>> topicsToRemove =
            new Dictionary<string, List<Action>>();

        private readonly Dictionary<string, bool> raising = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> adding = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> removing = 
            new Dictionary<string, bool>();

        public void Subscribe(string topic, Action callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Add(callback);
                }
                else
                {
                    topics.Add(topic, new List<Action>() {callback});
                }

                return;
            }

            if (!adding.ContainsKey(topic))
            {
                adding.Add(topic, true);
            }
            else
            {
                adding[topic] = true;
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                topicsToAdd[topic].Add(callback);
            }
            else
            {
                topicsToAdd.Add(topic, new List<Action> {callback});
            }

            if (topicsToRemove.ContainsKey(topic) && topicsToRemove[topic].Contains(callback))
            {
                topicsToRemove[topic].Remove(callback);
            }

            adding[topic] = false;
        }

        public void UnSubscribe(string topic, Action callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Remove(callback);
                }

                return;
            }

            if (!removing.ContainsKey(topic))
            {
                removing.Add(topic, true);
            }
            else
            {
                removing[topic] = true;
            }

            if (topicsToRemove.ContainsKey(topic))
            {
                topicsToRemove[topic].Add(callback);
            }
            else
            {
                topicsToRemove.Add(topic, new List<Action> {callback});
            }

            if (topicsToAdd.ContainsKey(topic) && topicsToAdd[topic].Contains(callback))
            {
                topicsToAdd[topic].Remove(callback);
            }

            removing[topic] = false;
        }

        public void RaiseEvent(string topic)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, true);
            }
            else
            {
                oldRaising = raising[topic];
                raising[topic] = true;
            }

            topicsToAddCache.CopyFrom(topicsToAdd);

            foreach (var item in topicsToAddCache)
            {
                if (!topics.ContainsKey(item.Key))
                {
                    topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    topics[item.Key].AddRange(item.Value);
                }

                topicsToAdd.Remove(item.Key);
            }

            foreach (var item in topicsToRemove)
            {
                if (topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        topics[item.Key].Remove(action);
                    }
                }
            }

            topicsToRemove.Clear();

            if (!topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in topics[topic])
                {
                    callback.InvokeSafe();
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message, LogType.EventChannel);
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                if (!((adding.ContainsKey(topic) && adding[topic]) ||
                      (removing.ContainsKey(topic) && removing[topic])))
                    try
                    {
                        foreach (var callback in topicsToAdd[topic])
                        {
                            callback.InvokeSafe();
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message, LogType.EventChannel);
                    }
                else
                {
                }
            }

            raising[topic] = false;
        }
    }

    public class EventChannel<TArg> : IEventChannel<TArg>
    {
        private readonly Dictionary<string, List<Action<TArg>>> topics =
            new Dictionary<string, List<Action<TArg>>>();

        private readonly Dictionary<string, List<Action<TArg>>> topicsToAdd =
            new Dictionary<string, List<Action<TArg>>>();

        private readonly Dictionary<string, List<Action<TArg>>> topicsToAddCache =
            new Dictionary<string, List<Action<TArg>>>();

        private readonly Dictionary<string, List<Action<TArg>>> topicsToRemove =
            new Dictionary<string, List<Action<TArg>>>();

        private readonly Dictionary<string, bool> raising = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> adding = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> removing = 
            new Dictionary<string, bool>();

        public void Subscribe(string topic, Action<TArg> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Add(callback);
                }
                else
                {
                    topics.Add(topic, new List<Action<TArg>>() {callback});
                }

                return;
            }

            if (!adding.ContainsKey(topic))
            {
                adding.Add(topic, true);
            }
            else
            {
                adding[topic] = true;
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                topicsToAdd[topic].Add(callback);
            }
            else
            {
                topicsToAdd.Add(topic, new List<Action<TArg>> {callback});
            }

            if (topicsToRemove.ContainsKey(topic) && topicsToRemove[topic].Contains(callback))
            {
                topicsToRemove[topic].Remove(callback);
            }

            adding[topic] = false;
        }

        public void UnSubscribe(string topic, Action<TArg> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Remove(callback);
                }

                return;
            }

            if (!removing.ContainsKey(topic))
            {
                removing.Add(topic, true);
            }
            else
            {
                removing[topic] = true;
            }

            if (topicsToRemove.ContainsKey(topic))
            {
                topicsToRemove[topic].Add(callback);
            }
            else
            {
                topicsToRemove.Add(topic, new List<Action<TArg>> {callback});
            }

            if (topicsToAdd.ContainsKey(topic) && topicsToAdd[topic].Contains(callback))
            {
                topicsToAdd[topic].Remove(callback);
            }

            removing[topic] = false;
        }

        public void RaiseEvent(string topic, TArg arg)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, true);
            }
            else
            {
                oldRaising = raising[topic];
                raising[topic] = true;
            }

            topicsToAddCache.CopyFrom(topicsToAdd);

            foreach (var item in topicsToAddCache)
            {
                if (!topics.ContainsKey(item.Key))
                {
                    topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    topics[item.Key].AddRange(item.Value);
                }

                topicsToAdd.Remove(item.Key);
            }

            foreach (var item in topicsToRemove)
            {
                if (topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        topics[item.Key].Remove(action);
                    }
                }
            }

            topicsToRemove.Clear();

            if (!topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in topics[topic])
                {
                    callback.InvokeSafe(arg);
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message, LogType.EventChannel);
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                if (!((adding.ContainsKey(topic) && adding[topic]) ||
                      (removing.ContainsKey(topic) && removing[topic])))
                    try
                    {
                        foreach (var callback in topicsToAdd[topic])
                        {
                            callback.InvokeSafe(arg);
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message, LogType.EventChannel);
                    }
                else
                {
                }
            }

            raising[topic] = false;
        }
    }

    public class EventChannel<TArg1, TArg2> : IEventChannel<TArg1, TArg2>
    {
        private readonly Dictionary<string, List<Action<TArg1, TArg2>>> topics =
            new Dictionary<string, List<Action<TArg1, TArg2>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2>>> topicsToAdd =
            new Dictionary<string, List<Action<TArg1, TArg2>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2>>> topicsToAddCache =
            new Dictionary<string, List<Action<TArg1, TArg2>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2>>> topicsToRemove =
            new Dictionary<string, List<Action<TArg1, TArg2>>>();

        private readonly Dictionary<string, bool> raising = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> adding = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> removing = 
            new Dictionary<string, bool>();

        public void Subscribe(string topic, Action<TArg1, TArg2> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Add(callback);
                }
                else
                {
                    topics.Add(topic, new List<Action<TArg1, TArg2>>() {callback});
                }

                return;
            }

            if (!adding.ContainsKey(topic))
            {
                adding.Add(topic, true);
            }
            else
            {
                adding[topic] = true;
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                topicsToAdd[topic].Add(callback);
            }
            else
            {
                topicsToAdd.Add(topic, new List<Action<TArg1, TArg2>> {callback});
            }

            if (topicsToRemove.ContainsKey(topic) && topicsToRemove[topic].Contains(callback))
            {
                topicsToRemove[topic].Remove(callback);
            }

            adding[topic] = false;
        }

        public void UnSubscribe(string topic, Action<TArg1, TArg2> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Remove(callback);
                }

                return;
            }

            if (!removing.ContainsKey(topic))
            {
                removing.Add(topic, true);
            }
            else
            {
                removing[topic] = true;
            }

            if (topicsToRemove.ContainsKey(topic))
            {
                topicsToRemove[topic].Add(callback);
            }
            else
            {
                topicsToRemove.Add(topic, new List<Action<TArg1, TArg2>> {callback});
            }

            if (topicsToAdd.ContainsKey(topic) && topicsToAdd[topic].Contains(callback))
            {
                topicsToAdd[topic].Remove(callback);
            }

            removing[topic] = false;
        }

        public void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            var oldRaising = false;

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, true);
            }
            else
            {
                oldRaising = raising[topic];
                raising[topic] = true;
            }

            topicsToAddCache.CopyFrom(topicsToAdd);

            foreach (var item in topicsToAddCache)
            {
                if (!topics.ContainsKey(item.Key))
                {
                    topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    topics[item.Key].AddRange(item.Value);
                }

                topicsToAdd.Remove(item.Key);
            }

            foreach (var item in topicsToRemove)
            {
                if (topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        topics[item.Key].Remove(action);
                    }
                }
            }

            topicsToRemove.Clear();

            if (!topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in topics[topic])
                {
                    callback.InvokeSafe(arg1, arg2);
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message, LogType.EventChannel);
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                if (!((adding.ContainsKey(topic) && adding[topic]) ||
                      (removing.ContainsKey(topic) && removing[topic])))
                    try
                    {
                        foreach (var callback in topicsToAdd[topic])
                        {
                            callback.InvokeSafe(arg1, arg2);
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message, LogType.EventChannel);
                    }
                else
                {
                }
            }

            raising[topic] = false;
        }
    }

    public class EventChannel<TArg1, TArg2, TArg3> : IEventChannel<TArg1, TArg2, TArg3>
    {
        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3>>> topics =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3>>> topicsToAdd =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3>>> topicsToAddCache =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3>>> topicsToRemove =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3>>>();

        private readonly Dictionary<string, bool> raising = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> adding = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> removing = 
            new Dictionary<string, bool>();

        public void Subscribe(string topic, Action<TArg1, TArg2, TArg3> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Add(callback);
                }
                else
                {
                    topics.Add(topic, new List<Action<TArg1, TArg2, TArg3>>() {callback});
                }

                return;
            }

            if (!adding.ContainsKey(topic))
            {
                adding.Add(topic, true);
            }
            else
            {
                adding[topic] = true;
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                topicsToAdd[topic].Add(callback);
            }
            else
            {
                topicsToAdd.Add(topic, new List<Action<TArg1, TArg2, TArg3>> {callback});
            }

            if (topicsToRemove.ContainsKey(topic) && topicsToRemove[topic].Contains(callback))
            {
                topicsToRemove[topic].Remove(callback);
            }

            adding[topic] = false;
        }

        public void UnSubscribe(string topic, Action<TArg1, TArg2, TArg3> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Remove(callback);
                }

                return;
            }

            if (!removing.ContainsKey(topic))
            {
                removing.Add(topic, true);
            }
            else
            {
                removing[topic] = true;
            }

            if (topicsToRemove.ContainsKey(topic))
            {
                topicsToRemove[topic].Add(callback);
            }
            else
            {
                topicsToRemove.Add(topic, new List<Action<TArg1, TArg2, TArg3>> {callback});
            }

            if (topicsToAdd.ContainsKey(topic) && topicsToAdd[topic].Contains(callback))
            {
                topicsToAdd[topic].Remove(callback);
            }

            removing[topic] = false;
        }

        public void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, true);
            }
            else
            {
                oldRaising = raising[topic];
                raising[topic] = true;
            }

            topicsToAddCache.CopyFrom(topicsToAdd);

            foreach (var item in topicsToAddCache)
            {
                if (!topics.ContainsKey(item.Key))
                {
                    topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    topics[item.Key].AddRange(item.Value);
                }

                topicsToAdd.Remove(item.Key);
            }

            foreach (var item in topicsToRemove)
            {
                if (topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        topics[item.Key].Remove(action);
                    }
                }
            }

            topicsToRemove.Clear();

            if (!topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in topics[topic])
                {
                    callback.InvokeSafe(arg1, arg2, arg3);
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message, LogType.EventChannel);
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                if (!((adding.ContainsKey(topic) && adding[topic]) ||
                      (removing.ContainsKey(topic) && removing[topic])))
                    try
                    {
                        foreach (var callback in topicsToAdd[topic])
                        {
                            callback.InvokeSafe(arg1, arg2, arg3);
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message, LogType.EventChannel);
                    }
                else
                {
                }
            }

            raising[topic] = false;
        }
    }

    public class EventChannel<TArg1, TArg2, TArg3, TArg4> : IEventChannel<TArg1, TArg2, TArg3, TArg4>
    {
        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>> topics =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>> topicsToAdd =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>> topicsToAddCache =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>>();

        private readonly Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>> topicsToRemove =
            new Dictionary<string, List<Action<TArg1, TArg2, TArg3, TArg4>>>();

        private readonly Dictionary<string, bool> raising = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> adding = 
            new Dictionary<string, bool>();
        
        private readonly Dictionary<string, bool> removing = 
            new Dictionary<string, bool>();

        public void Subscribe(string topic, Action<TArg1, TArg2, TArg3, TArg4> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Add(callback);
                }
                else
                {
                    topics.Add(topic, new List<Action<TArg1, TArg2, TArg3, TArg4>>() {callback});
                }

                return;
            }

            if (!adding.ContainsKey(topic))
            {
                adding.Add(topic, true);
            }
            else
            {
                adding[topic] = true;
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                topicsToAdd[topic].Add(callback);
            }
            else
            {
                topicsToAdd.Add(topic, new List<Action<TArg1, TArg2, TArg3, TArg4>> {callback});
            }

            if (topicsToRemove.ContainsKey(topic) && topicsToRemove[topic].Contains(callback))
            {
                topicsToRemove[topic].Remove(callback);
            }

            adding[topic] = false;
        }

        public void UnSubscribe(string topic, Action<TArg1, TArg2, TArg3, TArg4> callback)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, false);
            }

            var oldRaising = raising[topic];

            if (!oldRaising)
            {
                if (topics.ContainsKey(topic))
                {
                    topics[topic].Remove(callback);
                }

                return;
            }

            if (!removing.ContainsKey(topic))
            {
                removing.Add(topic, true);
            }
            else
            {
                removing[topic] = true;
            }

            if (topicsToRemove.ContainsKey(topic))
            {
                topicsToRemove[topic].Add(callback);
            }
            else
            {
                topicsToRemove.Add(topic, new List<Action<TArg1, TArg2, TArg3, TArg4>> {callback});
            }

            if (topicsToAdd.ContainsKey(topic) && topicsToAdd[topic].Contains(callback))
            {
                topicsToAdd[topic].Remove(callback);
            }

            removing[topic] = false;
        }

        public void RaiseEvent(string topic, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            bool oldRaising = false;

            if (!raising.ContainsKey(topic))
            {
                raising.Add(topic, true);
            }
            else
            {
                oldRaising = raising[topic];
                raising[topic] = true;
            }

            topicsToAddCache.CopyFrom(topicsToAdd);

            foreach (var item in topicsToAddCache)
            {
                if (!topics.ContainsKey(item.Key))
                {
                    topics.Add(item.Key, item.Value);
                }
                else
                {
                    if (oldRaising)
                    {
                        continue;
                    }

                    topics[item.Key].AddRange(item.Value);
                }

                topicsToAdd.Remove(item.Key);
            }

            foreach (var item in topicsToRemove)
            {
                if (topics.ContainsKey(item.Key))
                {
                    foreach (var action in item.Value)
                    {
                        topics[item.Key].Remove(action);
                    }
                }
            }

            topicsToRemove.Clear();

            if (!topics.ContainsKey(topic))
            {
                return;
            }

            try
            {
                foreach (var callback in topics[topic])
                {
                    callback.InvokeSafe(arg1, arg2, arg3, arg4);
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message, LogType.EventChannel);
            }

            if (topicsToAdd.ContainsKey(topic))
            {
                if (!((adding.ContainsKey(topic) && adding[topic]) ||
                      (removing.ContainsKey(topic) && removing[topic])))
                    try
                    {
                        foreach (var callback in topicsToAdd[topic])
                        {
                            callback.InvokeSafe(arg1, arg2, arg3, arg4);
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message, LogType.EventChannel);
                    }
                else
                {
                }
            }

            raising[topic] = false;
        }
    }
}
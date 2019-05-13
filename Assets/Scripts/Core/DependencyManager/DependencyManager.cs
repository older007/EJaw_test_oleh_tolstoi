using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Core.DependencyManager
{
    public static class DI
    {
        public static void Add<T>(object dependency, bool replace = true)
        {
            DependencyManager.AddDependency<T>(dependency,replace);
        }

        public static void Add(Type type, object dependency, bool replace = true)
        {
            DependencyManager.AddDependency(type,dependency,replace);
        }

        public static T Get<T>()
        {
            return DependencyManager.ResolveDependency<T>();
        }

        public static void Clear()
        {
            DependencyManager.Clear();
        }
    }

    public static class DependencyManager
    {
        private static Dictionary<Type, object> Dependencies = new Dictionary<Type, object>();

        private static void InitDependency(object dependency)
        {
            var initable = dependency as IInitable;

            initable?.Init();
        }

        public static void AddDependency<T>(object dependency, bool replace = true)
        {
            AddDependency(typeof(T), dependency, replace);
        }

        public static void AddDependency(Type type, object dependency, bool replace = true)
        {
            if (Dependencies.ContainsKey(type))
            {
                if (replace)
                {
                    Dependencies[type] = dependency;
                }

                return;
            }

            InitDependency(dependency);
            
            Dependencies.Add(type, dependency);
        }

        public static T ResolveDependency<T>()
        {
            if (Dependencies != null && Dependencies.ContainsKey(typeof(T)))
            {
                try
                {
                    return (T) Dependencies[typeof(T)];
                }
                catch
                {
                    Debug.LogError($"Can't resolve {typeof(T)}");
                }
            }

            return default(T);
        }

        public static void Clear()
        {
            Dependencies.Clear();
        }
    }
}
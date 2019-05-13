using System;
using System.Linq;

namespace Core.Utils
{
    public static class ActionExtensions
    {
        public static void InvokeSafe(this Action action, bool strict = false)
        {
            if (action == null)
            {
                return;
            }

            if (strict)
            {
                var delegates = action.GetInvocationList();

                if (delegates.Any(item => item.Target == null))
                {
                    return;
                }
            }

            action.Invoke();
        }

        public static void InvokeSafe<T>(this Action<T> action, T arg, bool strict = true)
        {
            if (action == null)
            {
                return;
            }

            if (strict)
            {
                var delegates = action.GetInvocationList();

                if (delegates.Any(item => item.Target == null))
                {
                    return;
                }
            }

            action.Invoke(arg);
        }

        public static void InvokeSafe<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2, bool strict = true)
        {
            if (action == null)
            {
                return;
            }

            if (strict)
            {
                var delegates = action.GetInvocationList();

                if (delegates.Any(item => item.Target == null))
                {
                    return;
                }
            }

            action.Invoke(arg1, arg2);
        }

        public static void InvokeSafe<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3,
            bool strict = true)
        {
            if (action == null)
            {
                return;
            }

            if (strict)
            {
                var delegates = action.GetInvocationList();

                if (delegates.Any(item => item.Target == null))
                {
                    return;
                }
            }

            action.Invoke(arg1, arg2, arg3);
        }

        public static void InvokeSafe<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, bool strict = true)
        {
            if (action == null)
            {
                return;
            }

            if (strict)
            {
                var delegates = action.GetInvocationList();

                if (delegates.Any(item => item.Target == null))
                {
                    return;
                }
            }

            action.Invoke(arg1, arg2, arg3, arg4);
        }
    }
}
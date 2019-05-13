using Core.Interfaces;
using UnityEngine;
using LogType = Core.Utils.Logger.LogType;

namespace Core.Utils
{
    public abstract class Initable : IInitable
    {
        private bool initable;

        void IInitable.Init()
        {
            if (initable)
            {
                Debug.LogError(($"{GetType().Name} : Initialization duplication", LogType.Core));

                return;
            }

            initable = true;

            
            Init();
            
            Debug.LogWarning($"{GetType().Name} : Inited");
        }

        protected abstract void Init();
    }
}
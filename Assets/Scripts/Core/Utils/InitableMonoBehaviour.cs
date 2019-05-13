using Core.Interfaces;
using UnityEngine;

namespace Core.Utils
{
    public abstract class InitableMonoBehaviour : MonoBehaviour, IInitable
    {
        private bool initable;

        protected virtual void Awake()
        {
            if (initable)
            {
                return;
            }

            initable = true;
            
            Init();
            
            Debug.LogWarning($"{this} : Inited");
        }

        void IInitable.Init()
        {
            if (initable)
            {
                return;
            }

            initable = true;

            Init();
            
            Debug.LogWarning($"{this} : Inited");
        }

        protected abstract void Init();
    }
}
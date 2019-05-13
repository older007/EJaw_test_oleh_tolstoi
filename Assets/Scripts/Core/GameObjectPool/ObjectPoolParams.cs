using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameObjectPool
{
    [System.Serializable]
    public class ObjectPoolParams
    {
        public readonly List<Action<GameObject>> OnFrameActions = new List<Action<GameObject>>();
        public bool Activate;
        public bool AutoExtend;
        public int PreWarmCount;
        [HideInInspector] public int Count;
        [HideInInspector] public int TakeFromPoolCount;

        public ObjectPoolParams()
        {
            
        }

        public ObjectPoolParams(ObjectPoolParams poolParams)
        {
            OnFrameActions = poolParams.OnFrameActions;
            Activate = poolParams.Activate;
            AutoExtend = poolParams.AutoExtend;
            Count = poolParams.Count;
            TakeFromPoolCount = poolParams.TakeFromPoolCount;
        }
    }
}
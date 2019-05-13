using Core.Interfaces;
using Core.Utils;
using Core.Utils.ScriptableObjects;
using UnityEngine;

namespace Core.GameObjectPool
{
    public class PrefabManager : Initable, IPrefabManager
    {
        public Prefabs Prefabs { get; private set; }

        private ObjectPool objectPool;
        private bool inited;
        
        public PrefabManager(ObjectPool objectPool, Prefabs prefabs)
        {
            this.objectPool = objectPool;
            Prefabs = prefabs;
            
            PreWarmPool();
        }

        protected override void Init()
        {
            PreWarmPool();            
        }

        private void PreWarmPool()
        {
            foreach (var item in Prefabs.PrefabObjects)
            {
                objectPool.PreWarmPool(item.Prefab, item.PrefabParams.PreWarmCount, item.PrefabParams);
            }
        }
    }
}
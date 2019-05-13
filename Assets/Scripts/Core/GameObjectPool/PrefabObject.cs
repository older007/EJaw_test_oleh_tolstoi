using UnityEngine;

namespace Core.GameObjectPool
{
    [System.Serializable]
    public class PrefabObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private ObjectPoolParams prefabParams;
        
        public GameObject Prefab => prefab;
        public ObjectPoolParams PrefabParams => prefabParams;
    }
}

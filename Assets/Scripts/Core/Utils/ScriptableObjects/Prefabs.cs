using System.Collections.Generic;
using Core.GameObjectPool;
using UnityEngine;

namespace Core.Utils.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Prefabs", fileName = "Prefabs")]
    public class Prefabs : ScriptableObject
    {
        [SerializeField] private List<PrefabObject> prefabObjects;

        public List<PrefabObject> PrefabObjects => prefabObjects;
    }
}

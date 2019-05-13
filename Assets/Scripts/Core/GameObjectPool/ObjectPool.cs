using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Utils;
using UniRx;
using Object = UnityEngine.Object;

namespace Core.GameObjectPool
{
    public class ObjectPool : Initable
    {
        private readonly float _maxUpdateTime = 0.1f;

        private readonly Dictionary<GameObject, List<GameObject>> objectPools =
            new Dictionary<GameObject, List<GameObject>>();

        private readonly Dictionary<GameObject, GameObject> objectsPrefabs = new Dictionary<GameObject, GameObject>();

        private readonly Dictionary<GameObject, ObjectPoolParams> poolParams =
            new Dictionary<GameObject, ObjectPoolParams>();

        private readonly List<GameObject> takenObjects = new List<GameObject>();
        private readonly Transform poolParent;
        private readonly GameObject gameObject;
        private float timeRunning;
        private Transform Transform => gameObject.transform;
        
        public ObjectPool(GameObject parentGm)
        {
            gameObject = parentGm;
            poolParent = parentGm.transform;
        }

        private void LateUpdate()
        {
            timeRunning = Time.realtimeSinceStartup;

            foreach (var param in poolParams)
            {
                if (param.Value.OnFrameActions.Count == 0)
                {
                    continue;
                }

                for (var i = 0; i < param.Value.OnFrameActions.Count; i++)
                {
                    var act = param.Value.OnFrameActions[0];
                    param.Value.OnFrameActions.RemoveAt(0);

                    act?.Invoke(param.Key);
                }
            }
        }

        private GameObject AddToPool(GameObject prefab, bool isNew = false)
        {
            var result = Object.Instantiate(prefab);
            result.name = $"{prefab.name}_{poolParams[prefab].Count++}";
            objectsPrefabs.Add(result, prefab);

            if (isNew)
            {
                takenObjects.Add(result);
            }

            result.SetActive(poolParams[prefab].Activate);

            return result;
        }

        private GameObject TakeFromPool(GameObject prefab)
        {
            poolParams[prefab].TakeFromPoolCount--;
            var go = objectPools[prefab][0];
            objectPools[prefab].RemoveAt(0);
            takenObjects.Add(go);
            go.SetActive(poolParams[prefab].Activate);
            go.transform.SetParent(null);
            go.SendMessage(Constants.OnTakenFromPool, SendMessageOptions.DontRequireReceiver);
            return go;
        }

        public void PreWarmPool(GameObject prefab, int poolSize, ObjectPoolParams @params)
        {
            if (prefab == null || poolParams.ContainsKey(prefab) || objectPools.ContainsKey(prefab))
            {
                return;
            }

            if (@params == null)
            {
                @params = new ObjectPoolParams();
            }

            objectPools.Add(prefab, new List<GameObject>());
            poolParams.Add(prefab, new ObjectPoolParams(@params));

            for (var i = 0; i < poolSize; i++)
            {
                objectPools[prefab].Add(AddToPool(prefab));
                objectPools[prefab][i].transform.SetParent(poolParent);
            }
        }

        public void Instantiate(GameObject prefab, Action<GameObject> action = null)
        {
            if (prefab == null)
            {
                return;
            }

            if (!poolParams.ContainsKey(prefab) || !objectPools.ContainsKey(prefab))
            {
                action?.Invoke(Object.Instantiate(prefab));
                return;
            }

            if (objectPools[prefab].Count - poolParams[prefab].TakeFromPoolCount <= 0)
            {
                if (!poolParams[prefab].AutoExtend)
                {
                    return;
                }

                poolParams[prefab].OnFrameActions.Add(go => action?.Invoke(AddToPool(go, true)));
            }
            else
            {
                if (Time.realtimeSinceStartup - timeRunning < _maxUpdateTime)
                {
                    poolParams[prefab].TakeFromPoolCount++;
                    action?.Invoke(TakeFromPool(prefab));
                }
                else
                {
                    poolParams[prefab].TakeFromPoolCount++;
                    poolParams[prefab].OnFrameActions.Add(go => action?.Invoke(TakeFromPool(go)));
                }
            }
        }

        public void Destroy(GameObject objectToDestroy)
        {
            if (!takenObjects.Contains(objectToDestroy))
            {
                if (!objectsPrefabs.ContainsKey(objectToDestroy))
                {
                    Object.Destroy(objectToDestroy);
                }

                return;
            }

            objectToDestroy.transform.SetParent(poolParent);
            objectToDestroy.SetActive(false);
            objectPools[objectsPrefabs[objectToDestroy]].Add(objectToDestroy);
            takenObjects.Remove(objectToDestroy);
        }

        protected override void Init()
        {
            Transform.SetParent(null);
            
            var count = Transform.childCount;

            for (var i = count - 1; i >= 0; i--)
            {
                Destroy(Transform.GetChild(i).gameObject);
            }
            
            poolParent.SetParent(Transform);
            poolParent.gameObject.SetActive(false);
            poolParent.name = Constants.ObjectPoolParent;

            Observable.EveryLateUpdate().Subscribe(x => { LateUpdate();});
        }
    }
}
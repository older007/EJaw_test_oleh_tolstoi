using System.Collections.Generic;
using Core.DependencyManager;
using Core.GameObjectPool;
using Core.Utils;
using UniRx;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : InitableMonoBehaviour
    {
        [SerializeField] private Transform[] enemyDots;
        [SerializeField] private GameObject[] prefabs;
        [SerializeField, Range(1,10)] private int maxCount;

        private List<Enemy> enemies = new List<Enemy>();

        private ObjectPool ObjectPool => DI.Get<ObjectPool>();

        protected override void Init()
        {
            this.GetChannel().Subscribe(Constants.Restart, Restart);
            this.GetChannel<Enemy>().Subscribe(Constants.DestroyEnemy, DestroyEnemy);
        }

        private void Restart()
        {
            foreach (var enemy in enemies)
            {
                DestroyEnemy(enemy);
            }
        }

        private void Start()
        {
            Observable.EveryUpdate().Where(w => enemies.Count < maxCount).Subscribe(x=> CreateNewEnemy());
        }

        private void CreateNewEnemy()
        {
            var index = Random.Range(0, prefabs.Length);
            
            ObjectPool.Instantiate(prefabs[index], EnemyInstantiated);
        }

        private void EnemyInstantiated(GameObject obj)
        {            
            var index = Random.Range(0, enemyDots.Length);

            var enemy = obj.GetComponent<Enemy>();
            
            enemies.Add(enemy);

            enemy.transform.position = enemyDots[index].position;
            enemy.Mover.StartPost = enemyDots[index].position;
            enemy.gameObject.SetActive(true);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            ObjectPool.Destroy(enemy.gameObject);

            enemies.Remove(enemy);
        }
    }
}
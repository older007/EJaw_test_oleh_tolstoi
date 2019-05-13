using Core.DependencyManager;
using Core.GameObjectPool;
using Core.Interfaces;
using Core.Managers;
using Core.Utils;
using Core.Utils.ScriptableObjects;
using Core.WebRequest;
using UnityEngine;
using Logger = Core.Utils.Logger.Logger;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private Prefabs prefabs;
        [SerializeField] private LoggerSettings loggerSettings;
        [SerializeField] private SceneSettings sceneSettings;
        
        private GameObject objectPoolGameObject;
        
        private GameObject ObjectPoolGameObject
        {
            get
            {
                if (objectPoolGameObject == null)
                {
                    objectPoolGameObject = new GameObject(Constants.ObjectPoolParent);
                    
                    DontDestroyOnLoad(objectPoolGameObject);
                }

                return objectPoolGameObject;
            }
        }

        private void Awake()
        {
            InstallBindings();
            Setup();
            
            this.GetChannel().Subscribe(Constants.ApplicationQuit, Quit);
            this.GetChannel().Subscribe(Constants.LoadGameScene, LoadGameScene);
            this.GetChannel().Subscribe(Constants.LoadMainScene, LoadMainScene);
        }

        private void LoadMainScene()
        {
            DI.Get<ISceneManager>().LoadMainScene();
        }

        private void LoadGameScene()
        {
            DI.Get<ISceneManager>().LoadGameScene();
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void Setup()
        {
            DontDestroyOnLoad(this);

            Application.targetFrameRate = 60;

            gameObject.AddComponent<RoutineManager>();
        }

        private void InstallBindings()
        {
            var logger = new Logger(loggerSettings);
            var apiBehaviour = new ApiBehaviour();
            var sceneManager = new SceneManager(sceneSettings);
            var objectPool = new ObjectPool(ObjectPoolGameObject);
            var prefabManager = new PrefabManager(objectPool, prefabs);
            
            DI.Add<Logger>(logger);
            DI.Add<ObjectPool>(objectPool);
            DI.Add<IApiManager>(apiBehaviour);
            DI.Add<ISceneManager>(sceneManager);
            DI.Add<IPrefabManager>(prefabManager);
        }
    }
}
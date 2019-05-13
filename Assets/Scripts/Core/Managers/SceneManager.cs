using System.Collections.Generic;
using Core.Interfaces;
using Core.Utils;
using Core.Utils.ScriptableObjects;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public class SceneManager : Initable, ISceneManager
    {
        private readonly List<Scene> scenes = new List<Scene>();
        private readonly SceneSettings sceneSettings;
        
        public SceneManager(SceneSettings settings)
        {
            sceneSettings = settings;
        }

        protected override void Init()
        {
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                
                scenes.Add(scene);
            }
        }

        public void LoadMainScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneSettings.MainScene);
        }

        public void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneSettings.GameScene);
        }
    }
}

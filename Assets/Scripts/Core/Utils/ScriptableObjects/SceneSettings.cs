using UnityEngine;

namespace Core.Utils.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Scene", fileName = "Scene Settings")]
    public class SceneSettings : ScriptableObject
    {
        [SerializeField] private string mainScene;
        [SerializeField] private string gameScene;

        public string MainScene => mainScene;
        public string GameScene => gameScene;
    }
}
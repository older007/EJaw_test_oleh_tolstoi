using Core.DependencyManager;
using Core.Utils;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : InitableMonoBehaviour, ITargerPosition
    {
        public Vector3 Position => transform.position;

        private Vector3 playerPos;
        private Vector3 nextPos;
        private Vector3 startPost;
        private IMover mover;
        private InitableMonoBehaviour initableMonoBehaviourImplementation;
        private DotManager DotManager => DI.Get<DotManager>();

        private void Start()
        {
            startPost = transform.position;
            mover.StartPost = startPost;
            mover.EndPos = DotManager.GetPosition(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            var dot = other.GetComponent<Dot>();

            if (!dot)
            {
                return;
            }
        
            mover.Timer = 0;
            mover.StartPost = transform.position;
            mover.EndPos = DotManager.GetPosition(dot.Index);
        }

        
        protected override void Init()
        {  
            mover = GetComponent<IMover>();

            this.GetChannel().Subscribe(Constants.Restart, Restart);
        }

        protected override void Awake()
        {
            DI.Add<ITargerPosition>(this);

            base.Awake();
        }

        private void Restart()
        {
            mover.StartPost = startPost;
            mover.EndPos = DotManager.GetPosition(0);
        }
    }
}

using Core.DependencyManager;
using Core.Interfaces;
using Core.Utils;
using UniRx;
using UnityEngine;

namespace Managers
{
    public class GameManager : InitableMonoBehaviour, IGameState
    {
        public bool GameState { get; private set; }

        private float timer;
        private int score;

        private void OnUpdate()
        {
            timer += Time.deltaTime;

            if (GameState)
            {
                this.GetChannel<int>().RaiseEvent(Constants.TimerUpdate, (int)(Constants.TimeLimit - timer));
                
                if (timer >= Constants.TimeLimit)
                {
                    this.GetChannel().RaiseEvent(Constants.EndGame);
                
                    GameState = false;
                }
            }
        }

        private void Restart()
        {
            timer = 0;
            score = 0;
            GameState = true;

            UpdateScore();
        }

        protected override void Init()
        {
            DI.Add<IGameState>(this);

            this.GetChannel().Subscribe(Constants.Restart, Restart);
            this.GetChannel().Subscribe(Constants.EnemyCollision, EnemyCollision);
            this.GetChannel<int>().Subscribe(Constants.DestroyEnemy, UpYouScore);

            
            Observable.EveryLateUpdate().Subscribe(x => { OnUpdate();});

            GameState = true;
            timer = 0;
        }

        private void UpYouScore(int point)
        {
            score += point;
            
            UpdateScore();
        }

        private void EnemyCollision()
        {
            score -= 1;
            
            UpdateScore();
        }

        private void UpdateScore()
        {
            if (!GameState)
            {
                return;
            }

            score = Mathf.Clamp(score, 0, 100000);
         
            this.GetChannel<int>().RaiseEvent(Constants.ScoreUpdate, score);
        }
    }
}

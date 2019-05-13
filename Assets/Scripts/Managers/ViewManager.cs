using System.Globalization;
using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ViewManager : InitableMonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text timerText;
        
        protected override void Init()
        {
            restartButton.onClick.AddListener(Restart);
            restartButton.gameObject.SetActive(false);
            
            this.GetChannel().Subscribe(Constants.EndGame, EndGame);
            this.GetChannel<int>().Subscribe(Constants.ScoreUpdate, ScoreUpdate);
            this.GetChannel<int>().Subscribe(Constants.TimerUpdate, TimerUpdate);
        }

        private void TimerUpdate(int obj)
        {
            timerText.text = obj.ToString(CultureInfo.InvariantCulture);
        }

        private void ScoreUpdate(int obj)
        {
            scoreText.text = obj.ToString();
        }

        private void EndGame()
        {
            restartButton.gameObject.SetActive(true);
        }

        private void Restart()
        {
            this.GetChannel().RaiseEvent(Constants.Restart);
            
            restartButton.gameObject.SetActive(false);            
        }
    }
}

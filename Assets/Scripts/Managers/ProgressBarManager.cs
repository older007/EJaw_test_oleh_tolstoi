using Core.DependencyManager;
using Core.Interfaces;
using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ProgressBarManager : MonoBehaviour
    {
        [SerializeField] private Text InfoText;
        private IApiManager ApiManager => DI.Get<IApiManager>();
        private string url = "https://postman-echo.com/get?foo1=bar1&foo2=bar2";
        private void Start()
        {
            InfoText.text = "Loading Data";
            
            ApiManager.GetRequest<string>(url, (s) =>
            {
                InfoText.text = "Data Loaded";
                
                Invoke(nameof(MoveLoadNextScene), 2f);
            });
        }

        private void MoveLoadNextScene()
        {
            this.GetChannel().RaiseEvent(Constants.LoadMainScene);   
        }
    }
}

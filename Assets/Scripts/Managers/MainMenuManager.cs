using System.Collections;
using System.Collections.Generic;
using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    private void Start()
    {
        playButton.onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        this.GetChannel().RaiseEvent(Constants.LoadGameScene);
    }
}

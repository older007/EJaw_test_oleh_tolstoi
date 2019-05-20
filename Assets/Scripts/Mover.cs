using System.Collections;
using System.Collections.Generic;
using Core.DependencyManager;
using Core.Interfaces;
using Core.Utils;
using Interfaces;
using Managers;
using UniRx;
using UnityEngine;

public class Mover : InitableMonoBehaviour, IMover
{
    [SerializeField] protected float speed = 1;

    public bool CanMove => DI.Get<IGameState>().GameState;
    public float Timer { get; set; }
    public Vector3 StartPost { get; set; }
    public Vector3 EndPos { get; set; }

    private void OnUpdate()
    {
        if (DI.Get<IGameState>() == null || !CanMove)
        {
            return;
        }
        
        Timer += Time.deltaTime;

        Move();
    }

    protected virtual void Move()
    {
        var pos = Vector3.Lerp(StartPost, EndPos, Timer * speed);

        pos.y = (1 / transform.localScale.y) * -1;

        transform.position = pos;
    }

    protected override void Init()
    {
        this.GetChannel().Subscribe(Constants.Restart, Restart);    
        
        Observable.EveryFixedUpdate().Subscribe(x => { OnUpdate();});
    }

    private void Restart()
    {
        Timer = 0;
        transform.position = StartPost;
    }

    private void OnEnable()
    {
        Restart();   
    }
}
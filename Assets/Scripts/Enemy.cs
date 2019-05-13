using System.Collections;
using System.Collections.Generic;
using Core.DependencyManager;
using Core.GameObjectPool;
using Core.Utils;
using Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    private IMover mover;
    
    public IMover Mover
    {
        get
        {
            if (mover == null)
            {
                mover = GetComponent<Mover>();
            }

            return mover;
        }
    }

    private ITargerPosition TargetPosition => DI.Get<ITargerPosition>();

    private void Start()
    {
        Observable.EveryUpdate().Subscribe(s => OnUpdate());
    }

    private void OnUpdate()
    {
        if (Mover == null)
        {
            return;
        }

        Mover.EndPos = TargetPosition.Position;
    }

    private void OnMouseDown()
    {
        if (!mover.CanMove)
        {
            return;
        }

        this.GetChannel<Enemy>().RaiseEvent(Constants.DestroyEnemy,this);            
        this.GetChannel<int>().RaiseEvent(Constants.DestroyEnemy,Constants.Score);
    }

    private void OnTriggerEnter(Collider other)
    {
        this.GetChannel().RaiseEvent(Constants.EnemyCollision);
        this.GetChannel<Enemy>().RaiseEvent(Constants.DestroyEnemy, this);
    }
}
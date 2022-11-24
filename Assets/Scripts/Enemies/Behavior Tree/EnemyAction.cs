using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyAction : Action
{
    protected Animator anim;

    public override void OnAwake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }
}

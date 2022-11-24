using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyConditional : Conditional
{
    protected Animator anim;

    public override void OnAwake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }
}

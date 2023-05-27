using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyAction : Action
{
    protected Rigidbody body;
    protected Animator animator;
    protected EnemyInfo info;

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        info = GetComponent<EnemyInfo>(); 
    }
}

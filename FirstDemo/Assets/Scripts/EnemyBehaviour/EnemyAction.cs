using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyAction : Action
{
    protected Rigidbody body;
    protected Animator animator;
    protected CharacterController controller;

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
}

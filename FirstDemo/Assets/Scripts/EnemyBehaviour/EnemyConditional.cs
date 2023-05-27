using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyConditional: Conditional
{
    protected Rigidbody body;
    protected Animator animator;
    protected Collider selfCollider;    // 用于移动的碰撞盒
    
    public override void OnAwake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        selfCollider = GetComponent<Collider>(); 
    }
}
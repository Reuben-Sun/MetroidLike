using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyMove : EnemyAction
{
    public bool lookRight;
    public float moveDistance = 5.0f;
    public float moveUseTime = 2.0f;
    public float rotateUseTime = 0.5f;
    
    private Tween wanderTween;
    private float startTime;
    
    public override void OnStart()
    {
        startTime = Time.time;
        wanderTween = DOVirtual.DelayedCall(0, StartWander, false);
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time > startTime + moveUseTime)
        {
            body.velocity = Vector3.zero;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    private void StartWander()
    {
        float direction = lookRight ? 1 : -1;
        body.AddForce(Vector3.right * direction * moveDistance, ForceMode.Impulse);
        // transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.right * direction), rotateUseTime);
    }   
}

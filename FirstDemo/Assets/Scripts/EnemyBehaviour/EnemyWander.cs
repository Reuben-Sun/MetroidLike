// 敌人在出生点附近徘徊

using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyWander: EnemyAction
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
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    private void StartWander()
    {
        float direction = lookRight ? 1 : -1;
        transform.DOMove(new Vector3(transform.position.x + moveDistance * direction, transform.position.y, transform.position.z), moveUseTime);
        transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.right * direction), rotateUseTime);
    }
}
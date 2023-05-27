using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyJump : EnemyAction
{
    public float horizontalForce = 5.0f;
    public float jumpForce = 10.0f;

    public float buildupTime;
    public float jumpTime;

    public string animationTriggerName;

    private bool hasLanded; // 是否已经着地
    private Tween buildupTween;
    private Tween jumpTween;

    public override void OnStart()
    {
        buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
        animator.SetTrigger(animationTriggerName);
    }

    public override TaskStatus OnUpdate()
    {
        return hasLanded ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        jumpTween?.Kill();
        buildupTween?.Kill();
        hasLanded = false;
    }

    private void StartJump()
    {
        var direction = controller.transform.position.x < transform.position.x ? -1 : 1;
        body.AddForce(new Vector3(horizontalForce * direction, jumpForce, 0), ForceMode.Impulse);
        jumpTween = DOVirtual.DelayedCall(jumpTime, () => hasLanded = true, false);
    }
}

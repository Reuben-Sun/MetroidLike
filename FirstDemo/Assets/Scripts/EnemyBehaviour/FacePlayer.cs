using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// 敌人会朝向角色
public class FacePlayer: EnemyAction
{
    public override TaskStatus OnUpdate()
    {
        float direction = PlayerInfo.Instance.PlayerPosition.x - transform.position.x > 0 ? 1 : -1;

        transform.rotation = Quaternion.LookRotation(Vector3.right * direction);

        return TaskStatus.Success;
    }
}
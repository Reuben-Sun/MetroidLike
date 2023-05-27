using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsHealthUnder : EnemyConditional
{
    public float HealthTreshold;

    public override TaskStatus OnUpdate()
    {
        return PlayerInfo.Instance.Health < HealthTreshold ? TaskStatus.Success : TaskStatus.Failure;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(EmemyStateMechine _stateMechine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMechine, _enemyBase, _animBoolName, _enemy)
    {
        
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMechine.ChangeState(enemy.moveState);
        }

        
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

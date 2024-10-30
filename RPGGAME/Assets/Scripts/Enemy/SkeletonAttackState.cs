using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleAtteckState : EnemyState
{
    private Enemy_Skeleton enemy;
    
    public SkeleAtteckState(EmemyStateMechine _stateMechine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMechine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMechine.ChangeState(enemy.battleState);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    
    public SkeletonStunnedState(EmemyStateMechine _stateMechine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMechine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMechine.ChangeState(enemy.idleState);
        
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0f, 0.1f);
        stateTimer = enemy.stunDuration;
        
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelRedBlink", 0);
    }
}





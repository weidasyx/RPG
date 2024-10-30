using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;
    
    public SkeletonGroundedState(EmemyStateMechine _stateMechine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMechine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDectected() || Vector2.Distance(enemy.transform.position, player.position) < 2f)
        {
            stateMechine.ChangeState(enemy.battleState);
        }
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}

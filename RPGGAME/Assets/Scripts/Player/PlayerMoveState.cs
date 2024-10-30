using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //This not very good.
        // if (player.IsWallDetected())
        // {
        //     stateMechine.ChangeState(player.idleState);
        // }
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (xInput == 0 || player.IsWallDetected())
        {
            stateMechine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

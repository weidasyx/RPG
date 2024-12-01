using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (player.canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            player.canDoubleJump = false;
            stateMechine.ChangeState(player.jumpState);
        }
        if (player.IsWallDetected())
        {
            stateMechine.ChangeState(player.wallSlide);
        }
        if (player.IsGroundDetected())
        {
            stateMechine.ChangeState(player.idleState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
        }

      
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    
}


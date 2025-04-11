using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() == false)
        {
            stateMechine.ChangeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMechine.ChangeState(player.wallJump);
            return;
        }
        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMechine.ChangeState(player.idleState);
        }
        
        if (yInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y * .8f);
        }

        if (player.IsGroundDetected())
        {
            stateMechine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Jump();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
        {
            stateMechine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.canDoubleJump)
        {
            player.canDoubleJump = false;
            Jump();
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}

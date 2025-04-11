using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    
    public PlayerDashState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMechine.ChangeState(player.wallSlide);
        }
        
        player.SetVelocity(player.dashSpeed * player.dashDir,0);
        if (stateTimer < 0)
        {
            stateMechine.ChangeState(player.idleState);
        }
        player.fx.CreateAfterImage();
    }

    public override void Exit()
    {
        base.Exit();
        
        player.skill.dash.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);
        player.stats.MakeInvincible(false);
        
        
    }
}

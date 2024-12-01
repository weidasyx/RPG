using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
               if (player.skill.blackhole.CanUseSkill())
                   skillUsed = true;
                
            }
        }
        
        if (player.skill.blackhole.SkillCompleted())
            stateMechine.ChangeState(player.airState);

        
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.MakeTransParent(false);
    }
    
    

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.canDoubleJump = true;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackHoleUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("CoolDown!");
                return;
            }

            
            stateMechine.ChangeState(player.blackhole);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
        {
            stateMechine.ChangeState(player.aimSword);
        }
        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
        {
            stateMechine.ChangeState(player.counterAttackState);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            stateMechine.ChangeState(player.PrimaryAttackState);
        }
        if (!player.IsGroundDetected())
        {
            stateMechine.ChangeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMechine.ChangeState(player.jumpState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

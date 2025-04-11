using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    
    private float lastTimeAttecked;
    private float comboWindow = 2;
    
    public PlayerPrimaryAttackState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // AudioManager.instance.PlaySFX(2);
        xInput = 0;
        if (comboCounter > 2 || Time.time >= lastTimeAttecked + comboWindow)
        {
            comboCounter = 0;
        }
        
        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose atteck direction

        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;
        

        #endregion
        player.anim.speed = 1.2f;
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        // rb.velocity = Vector2.zero;
        if (stateTimer < 0)
            player.SetZeroVelocity();
        

        if (triggerCalled)
        {
            stateMechine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
        player.anim.speed = 1;
        comboCounter++;
        lastTimeAttecked = Time.time;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMechine _stateMechine, string _animBoolName) : base(_player, _stateMechine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();
    }

    public override void Update()
    {
        base.Update();
        
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}

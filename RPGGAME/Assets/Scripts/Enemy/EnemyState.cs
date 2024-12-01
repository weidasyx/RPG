using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyState
{
    protected EmemyStateMechine stateMechine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    
    private string animBoolName;
    
    protected float stateTimer;
    protected bool triggerCalled;


    public EnemyState(EmemyStateMechine _stateMechine, Enemy _enemyBase, string _animBoolName)
    {
        this.stateMechine = _stateMechine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    
    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }
    
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}

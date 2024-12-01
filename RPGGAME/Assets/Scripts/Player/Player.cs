using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }

    [Header("Attack details")] 
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.4f;
    

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;
    public bool canDoubleJump;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    
    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }


    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }







    #region States
    public PlayerStateMechine stateMechine { get; private set; }
    
    public PlayerMoveState moveState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackHoleState blackhole { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMechine = new PlayerStateMechine();

        idleState = new PlayerIdleState(this, stateMechine, "Idle");
        moveState = new PlayerMoveState(this, stateMechine, "Move");
        jumpState = new PlayerJumpState(this, stateMechine, "Jump");
        airState  = new PlayerAirState(this, stateMechine, "Jump");
        dashState = new PlayerDashState(this, stateMechine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMechine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMechine, "Jump");
        PrimaryAttackState = new PlayerPrimaryAttackState(this, stateMechine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMechine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMechine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMechine, "CatchSword");
        blackhole = new PlayerBlackHoleState(this, stateMechine, "Jump");
        deadState = new PlayerDeadState(this, stateMechine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMechine.Initialize(idleState);
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        
    }

    

    protected override void Update()
    {
        base.Update();
        stateMechine.currentState.Update();
        // Debug.Log(IsWallDetected());
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();
        // StartCoroutine("BusyFor", .1f);

    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        
        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMechine.ChangeState(catchSword);
        Destroy(sword);
    }

    public void ExitBlackHoleAbility()
    {
        stateMechine.ChangeState(airState);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMechine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;
            stateMechine.ChangeState(dashState);
            
        }
    }

    public override void Die()
    {
        base.Die();
        
        stateMechine.ChangeState(deadState);
    }
}

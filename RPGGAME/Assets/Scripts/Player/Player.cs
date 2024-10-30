using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }

    [Header("Attack details")] 
    public Vector2[] attackMovement;
    

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    

    

    
    
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
        
    }

    protected override void Start()
    {
        base.Start();
        
        stateMechine.Initialize(idleState);
        
    }

    

    protected override void Update()
    {
        base.Update();
        stateMechine.currentState.Update();
        // Debug.Log(IsWallDetected());
        CheckForDashInput();
        // StartCoroutine("BusyFor", .1f);

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
        dashUsageTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer <= 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;
            stateMechine.ChangeState(dashState);
            
        }
    }

    
    

    
    
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region States

    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeleAtteckState atteckState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        
        idleState = new SkeletonIdleState(stateMechine, this, "Idle", this);
        moveState = new SkeletonMoveState(stateMechine, this, "Move", this);
        battleState = new SkeletonBattleState(stateMechine, this, "Move", this);
        atteckState = new SkeleAtteckState(stateMechine, this, "Attack", this);
        stunnedState = new SkeletonStunnedState(stateMechine, this, "Stun", this);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.U))
        {
            stateMechine.ChangeState(stunnedState);
        }
    }

    protected override void Start()
    {
        base.Start();
        stateMechine.Initialize(idleState);
    }
}
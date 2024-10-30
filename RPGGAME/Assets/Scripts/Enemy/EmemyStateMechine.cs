using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmemyStateMechine
{
    public EnemyState currentState { get; private set; }

    public void Initialize(EnemyState _startstate)
    {
        currentState = _startstate;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}

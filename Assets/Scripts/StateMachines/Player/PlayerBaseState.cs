using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected PlayerStateMachine StateMachine { get; }
}

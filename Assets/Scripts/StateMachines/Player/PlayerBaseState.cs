using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        PlayerStateMachine = playerStateMachine;
    }

    protected PlayerStateMachine PlayerStateMachine { get; }
}

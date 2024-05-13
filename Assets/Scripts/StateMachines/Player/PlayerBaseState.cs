using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        PlayerStateMachine = playerStateMachine;
    }

    protected PlayerStateMachine PlayerStateMachine { get; }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        PlayerStateMachine.CharacterController.Move((motion + PlayerStateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (PlayerStateMachine.Targeter.CurrentTarget == null)
            return;

        Vector3 lookPos = PlayerStateMachine.Targeter.CurrentTarget.transform.position - PlayerStateMachine.transform.position;
        lookPos.y = 0f;
        PlayerStateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ReturnToLocomotion()
    {
        if (PlayerStateMachine.Targeter.CurrentTarget != null)
        {
            PlayerStateMachine.SwitchState(new PlayerTargetingState(PlayerStateMachine));
        }
        else
        {
            PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine));
        }
    }
}

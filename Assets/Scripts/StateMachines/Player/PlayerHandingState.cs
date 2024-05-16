using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly int HandingHash = Animator.StringToHash("Handing");
    
    private Vector3 _ledgeForward;
    private Vector3 _closestPoint;

    public PlayerHandingState(PlayerStateMachine playerStateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(playerStateMachine) 
    {
        _ledgeForward = ledgeForward;
        _closestPoint = closestPoint;
    }

    public override void Enter()
    {
        PlayerStateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);
        PlayerStateMachine.CharacterController.enabled = false;
        PlayerStateMachine.transform.position = _closestPoint - (PlayerStateMachine.LedgeDetector.transform.position - PlayerStateMachine.transform.position);
        PlayerStateMachine.CharacterController.enabled = true;
        PlayerStateMachine.Animator.CrossFadeInFixedTime(HandingHash, CrossFadeDuration);
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        if (PlayerStateMachine.InputReader.MovementValue.y > 0f)
        {
            PlayerStateMachine.SwitchState(new PlayerPullUpState(PlayerStateMachine));
        }
        else if (PlayerStateMachine.InputReader.MovementValue.y < 0f)
        {
            PlayerStateMachine.CharacterController.Move(Vector3.zero);
            PlayerStateMachine.ForceReceiver.Reset();
            PlayerStateMachine.SwitchState(new PlayerFallingState(PlayerStateMachine));
        }
    }
}
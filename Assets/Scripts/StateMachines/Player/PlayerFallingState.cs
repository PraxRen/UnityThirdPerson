using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int FallHash = Animator.StringToHash("Fall");

    private Vector3 _momentum;

    public PlayerFallingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    public override void Enter() 
    {
        _momentum = PlayerStateMachine.CharacterController.velocity;
        _momentum.y = 0f;
        PlayerStateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        PlayerStateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Exit() 
    {
        PlayerStateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime) 
    {
        Move(_momentum, deltaTime);

        if (PlayerStateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        PlayerStateMachine.SwitchState(new PlayerHandingState(PlayerStateMachine, ledgeForward, closestPoint));
    }
}

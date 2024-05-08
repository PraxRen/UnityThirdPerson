using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly static int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly static int TargetingRightHash = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        PlayerStateMachine.InputReader.CancelEvent += OnCancel;
        PlayerStateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        PlayerStateMachine.InputReader.CancelEvent -= OnCancel;
    }

    public override void Tick(float deltaTime)
    {
        if (PlayerStateMachine.InputReader.IsAttacking)
        {
            PlayerStateMachine.SwitchState(new PlayerAttackingState(PlayerStateMachine, 0));
            return;
        }

        if (PlayerStateMachine.Targeter.CurrentTarget == null)
        {
            PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * PlayerStateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }

    private void OnCancel()
    {
        PlayerStateMachine.Targeter.Cancel();
        PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += PlayerStateMachine.transform.right * PlayerStateMachine.InputReader.MovementValue.x;
        movement += PlayerStateMachine.transform.forward * PlayerStateMachine.InputReader.MovementValue.y;
        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        float targetingForward = 0f;
        float targetingRight = 0f;
        float dampTime = 0.1f;

        if (PlayerStateMachine.InputReader.MovementValue.y != 0)
        {
            targetingForward = PlayerStateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
        }

        if (PlayerStateMachine.InputReader.MovementValue.x != 0)
        {
            targetingRight = PlayerStateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
        }

        PlayerStateMachine.Animator.SetFloat(TargetingForwardHash, targetingForward, dampTime, deltaTime);
        PlayerStateMachine.Animator.SetFloat(TargetingRightHash, targetingRight, dampTime, deltaTime);
    }
}

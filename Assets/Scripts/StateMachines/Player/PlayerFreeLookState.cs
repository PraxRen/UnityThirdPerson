using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private readonly static int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly static int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        PlayerStateMachine.InputReader.TargetEvent += OnTarget;
        PlayerStateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        PlayerStateMachine.InputReader.TargetEvent -= OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        if (PlayerStateMachine.InputReader.IsAttacking)
        {
            PlayerStateMachine.SwitchState(new PlayerAttackingState(PlayerStateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * PlayerStateMachine.FreeLookMovementSpeed, deltaTime);

        if (PlayerStateMachine.InputReader.MovementValue == Vector2.zero)
        {
            PlayerStateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        PlayerStateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        PlayerStateMachine.transform.rotation = Quaternion.Lerp(PlayerStateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * PlayerStateMachine.RotationDamping);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = PlayerStateMachine.MainCameraTransform.forward;
        Vector3 right = PlayerStateMachine.MainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        return forward * PlayerStateMachine.InputReader.MovementValue.y + right * PlayerStateMachine.InputReader.MovementValue.x;
    }

    private void OnTarget()
    {
        if (PlayerStateMachine.Targeter.SelectTarget() == false)
            return;

        PlayerStateMachine.SwitchState(new PlayerTargetingState(PlayerStateMachine));
    }
}

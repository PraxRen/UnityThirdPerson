using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private const float AnimatorDampTime = 0.1f;
    private readonly static int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        PlayerStateMachine.CharacterController.Move(movement * PlayerStateMachine.FreeLookMovementSpeed * deltaTime);

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
}

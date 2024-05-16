using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int JumpHash = Animator.StringToHash("Jump");

    private Vector3 _momentum;

    public PlayerJumpingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    public override void Enter() 
    {
        PlayerStateMachine.ForceReceiver.Jump(PlayerStateMachine.JumpForce);
        _momentum = PlayerStateMachine.CharacterController.velocity;
        _momentum.y = 0f;
        PlayerStateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        PlayerStateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Exit() 
    {
        PlayerStateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime) 
    {
        Move(_momentum, deltaTime);

        if (PlayerStateMachine.CharacterController.velocity.y <= 0)
        {
            PlayerStateMachine.SwitchState(new PlayerFallingState(PlayerStateMachine));
            return;
        }

        FaceTarget();
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        PlayerStateMachine.SwitchState(new PlayerHandingState(PlayerStateMachine, ledgeForward, closestPoint));
    }
}

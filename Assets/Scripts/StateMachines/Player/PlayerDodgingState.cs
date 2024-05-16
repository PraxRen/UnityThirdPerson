using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly static int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly static int DodgeRightHash = Animator.StringToHash("DodgeRight");

    private float _remainingDodgeTime;
    private Vector3 _dodgingDirectionInput;

    public PlayerDodgingState(PlayerStateMachine playerStateMachine, Vector3 dodgingDirectionInput) : base(playerStateMachine) 
    {
        _dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter() 
    {
        //if (Time.time - PlayerStateMachine.PreviousDodgeTime < PlayerStateMachine.DodgeCooldown)
        //    return;

        //PlayerStateMachine.SetDodgeTime(Time.time);
        _remainingDodgeTime = PlayerStateMachine.DodgeDuration;
        PlayerStateMachine.Animator.SetFloat(DodgeForwardHash, _dodgingDirectionInput.y);
        PlayerStateMachine.Animator.SetFloat(DodgeRightHash, _dodgingDirectionInput.x);
        PlayerStateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);
        PlayerStateMachine.Health.SetInvulnerable(true);
    }

    public override void Exit() 
    {
        PlayerStateMachine.Health.SetInvulnerable(false);
    }

    public override void Tick(float deltaTime) 
    {
        Vector3 movement = new Vector3();
        movement += PlayerStateMachine.transform.right * _dodgingDirectionInput.x * PlayerStateMachine.DodgeLength / PlayerStateMachine.DodgeDuration;
        movement += PlayerStateMachine.transform.forward * _dodgingDirectionInput.y * PlayerStateMachine.DodgeLength / PlayerStateMachine.DodgeDuration;
        _remainingDodgeTime -= deltaTime;

        if (_remainingDodgeTime <= 0f)
        {
            PlayerStateMachine.SwitchState(new PlayerTargetingState(PlayerStateMachine));
        }

        Move(movement, deltaTime);
        FaceTarget();
    }
}
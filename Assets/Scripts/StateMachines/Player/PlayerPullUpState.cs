using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int PullUpHash = Animator.StringToHash("PullUp");
    private readonly static Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);

    public PlayerPullUpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        PlayerStateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        PlayerStateMachine.CharacterController.Move(Vector3.zero);
        PlayerStateMachine.ForceReceiver.Reset();
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(PlayerStateMachine.Animator, "Climbing") < 1f)
            return;
        PlayerStateMachine.CharacterController.enabled = false;
        PlayerStateMachine.transform.Translate(Offset, Space.Self);
        PlayerStateMachine.CharacterController.enabled = true;
        PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine, false));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly static int BlockHash = Animator.StringToHash("Block");

    public PlayerBlockingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter() 
    {
        PlayerStateMachine.Health.SetInvulnerable(true);
        PlayerStateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Exit() 
    {
        PlayerStateMachine.Health.SetInvulnerable(false);
    }

    public override void Tick(float deltaTime) 
    {
        Move(deltaTime);

        if (!PlayerStateMachine.InputReader.IsBlocking)
        {
            PlayerStateMachine.SwitchState(new PlayerTargetingState(PlayerStateMachine));
            return;
        }

        if(PlayerStateMachine.Targeter.CurrentTarget == null)
        {
            PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine));
            return;
        }
    }
}
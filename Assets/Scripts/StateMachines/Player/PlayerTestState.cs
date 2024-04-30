using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    private float _timer;

    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        StateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Exit()
    {
        StateMachine.InputReader.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;
        Debug.Log(_timer);
    }

    private void OnJump()
    {
        StateMachine.SwithState(new PlayerTestState(StateMachine));
    }
}

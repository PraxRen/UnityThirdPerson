using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private float _duration = 1f;

    public EnemyImpactState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        EnemyStateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            EnemyStateMachine.SwitchState(new EnemyIdleState(EnemyStateMachine));
        }
    }
}

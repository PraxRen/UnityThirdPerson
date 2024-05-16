using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private const float TransitionDuration = 0.1f;

    private readonly int AttackHash = Animator.StringToHash("Attack");

    public EnemyAttackingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter() 
    {
        EnemyStateMachine.WeaponDamage.SetAttack(EnemyStateMachine.AttackDamage, EnemyStateMachine.AttackKnockback);
        EnemyStateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) 
    {
        if (GetNormalizedTime(EnemyStateMachine.Animator, "Attack") >= 1)
        {
            EnemyStateMachine.SwitchState(new EnemyChasingState(EnemyStateMachine));
        }

        FacePlayer();
    }
}

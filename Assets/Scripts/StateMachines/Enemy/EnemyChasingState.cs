using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private readonly static int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly static int SpeedHash = Animator.StringToHash("Speed");

    public EnemyChasingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        EnemyStateMachine.NavMeshAgent.ResetPath();
        EnemyStateMachine.NavMeshAgent.velocity = Vector3.zero;
        EnemyStateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        EnemyStateMachine.NavMeshAgent.ResetPath();
        EnemyStateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            EnemyStateMachine.SwitchState(new EnemyIdleState(EnemyStateMachine));
            return;
        }

        if (IsInAttackRange())
        {
            EnemyStateMachine.SwitchState(new EnemyAttackingState(EnemyStateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer();
        EnemyStateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private bool IsInAttackRange()
    {
        if (EnemyStateMachine.HealthPlayer.IsDead)
            return false;

        float playerDistanceSqr = (EnemyStateMachine.HealthPlayer.transform.position - EnemyStateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= EnemyStateMachine.AttackRange * EnemyStateMachine.AttackRange;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (EnemyStateMachine.NavMeshAgent.isOnNavMesh)
        {
            EnemyStateMachine.NavMeshAgent.destination = EnemyStateMachine.HealthPlayer.transform.position;
            Vector3 motion = EnemyStateMachine.NavMeshAgent.desiredVelocity.normalized * EnemyStateMachine.MovementSpeed;
            Move(motion, deltaTime);
        }

        EnemyStateMachine.NavMeshAgent.velocity = EnemyStateMachine.CharacterController.velocity;
    }
}

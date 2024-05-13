using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    public EnemyBaseState(EnemyStateMachine enemyStateMachine)
    {
        EnemyStateMachine = enemyStateMachine;
    }

    protected EnemyStateMachine EnemyStateMachine { get; }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        EnemyStateMachine.CharacterController.Move((motion + EnemyStateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (EnemyStateMachine.HealthPlayer == null)
            return;

        Vector3 lookPos = EnemyStateMachine.HealthPlayer.transform.position - EnemyStateMachine.transform.position;
        lookPos.y = 0f;
        EnemyStateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        if (EnemyStateMachine.HealthPlayer.IsDead)
            return false;

        float playerDistanceSqr = (EnemyStateMachine.HealthPlayer.transform.position - EnemyStateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= EnemyStateMachine.PlayerChasingRange * EnemyStateMachine.PlayerChasingRange;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter() 
    {
        EnemyStateMachine.Ragdoll.ToggleRagdoll(true);
        EnemyStateMachine.WeaponDamage.gameObject.SetActive(false);
        GameObject.Destroy(EnemyStateMachine.Target);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}

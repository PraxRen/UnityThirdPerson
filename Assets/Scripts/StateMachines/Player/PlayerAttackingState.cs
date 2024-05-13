using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float _previousFrameTime;
    private bool _alreadyAppliedForce;
    private Attack _attack;

    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine) 
    {
        _attack = playerStateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        PlayerStateMachine.WeaponDamage.SetAttack(_attack.Damage, _attack.Knockback);
        PlayerStateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();
        float normalizedTime = GetNormalizedTime(PlayerStateMachine.Animator);

        if (normalizedTime >= _previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= _attack.ForceTime)
            {
                TryApplyForce();
            }

            if (PlayerStateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (PlayerStateMachine.Targeter.CurrentTarget != null)
            {
                PlayerStateMachine.SwitchState(new PlayerTargetingState(PlayerStateMachine));
            }
            else
            {
                PlayerStateMachine.SwitchState(new PlayerFreeLookState(PlayerStateMachine));
            }
        }

        _previousFrameTime = normalizedTime;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (_attack.ComboStateIndex == -1)
            return;

        if (normalizedTime < _attack.ComboAttackTime)
            return;

        PlayerStateMachine.SwitchState(new PlayerAttackingState(PlayerStateMachine, _attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if (_alreadyAppliedForce)
            return;

        PlayerStateMachine.ForceReceiver.AddForce(PlayerStateMachine.transform.forward * _attack.Force);
        _alreadyAppliedForce = true;
    }
}

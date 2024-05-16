using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _drag = 0.3f;

    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    private float _verticalVelocity;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

    private void Update()
    {
        if (_verticalVelocity < 0f && _characterController.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);

        if (_agent != null && _impact.sqrMagnitude < 0.2f * 0.2f)
        {
            _impact = Vector3.zero;
            _agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;

        if (_agent != null)
        {
            _agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        _impact = Vector3.zero;
        _verticalVelocity = 0f;
    }
}

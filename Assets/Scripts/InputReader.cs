using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    private Controls _controls;

    public event Action JumpEvent;
    public event Action DodgeEvent;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDestroy()
    {
        _controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed == false)
            return;

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed == false)
            return;

        DodgeEvent?.Invoke();
    }
}

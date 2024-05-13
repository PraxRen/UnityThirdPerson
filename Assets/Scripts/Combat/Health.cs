using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _health;
    private bool _isInvulnerable;

    public bool IsDead => _health == 0;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        _isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        if (IsDead)
            return;

        if (_isInvulnerable)
            return;

        _health = Mathf.Max(_health - damage, 0);
        OnTakeDamage?.Invoke();

        if (IsDead)
            OnDie?.Invoke();

        Debug.Log(_health);
    }
}
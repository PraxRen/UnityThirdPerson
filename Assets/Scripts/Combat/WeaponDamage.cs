using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> _alreadyColliderWith = new List<Collider>();
    private int _damage;
    private float _knockback;

    private void OnEnable()
    {
        _alreadyColliderWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myCollider == other)
            return;

        if (_alreadyColliderWith.Contains(other))
            return;

        _alreadyColliderWith.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            health.DealDamage(_damage);
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * _knockback);
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        _damage = damage;
        _knockback = knockback;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> _alreadyColliderWith = new List<Collider>();
    private int _damage;

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
            Debug.Log(other.transform.name);
            health.DealDamage(_damage);
        }
    }

    public void SetAttack(int damage)
    {
        _damage = damage;
    }
}

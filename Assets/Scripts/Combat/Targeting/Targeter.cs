using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cineTargetGroup;

    private Camera _mainCamera;
    private List<Target> _targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target) == false)
            return;

        _targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Target target) == false)
            return;

        RemoveTarget(target);
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0)
            return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
                continue;

            Vector2 toCenter = viewPos = new Vector2(0.5f, 0.5f);

            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null)
            return false;

        CurrentTarget = closestTarget;
        float weight = 1f;
        float radius = 2f;
        _cineTargetGroup.AddMember(CurrentTarget.transform, weight, radius);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null)
            return;

        _cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            _cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(UnitMovement), typeof(Collector))]
public class Unit : MonoBehaviour
{
    private Collector _collector;
    private UnitMovement _movement;
    private Resource _targetResource;

    public event Action<Unit> ReachedResource;
    public event Action<Unit> ReachedBase;

    public bool IsBusy => _collector.IsBusy;

    private void Awake()
    {
        _collector = GetComponent<Collector>();
        _movement = GetComponent<UnitMovement>();
    }

    public void GoToResources(Resource resource)
    {
        _targetResource = resource;

        MoveTo(resource.ArrivalPoint);
        _movement.Reached += OnReachedResource;
    }

    public void GoToBase(Transform @base)
    {
        MoveTo(@base);
        _movement.Reached += OnReachedBase;
    }

    public Resource GiveResource()
    {
        _targetResource = null;
        return _collector.GiveResource();
    }

    private void MoveTo(Transform target)
    {
        _movement.SetTarget(target);
        _movement.Move();
    }

    private void OnReachedResource()
    {
        _movement.Reached -= OnReachedResource;

        _collector.Take(_targetResource);

        ReachedResource?.Invoke(this);
    }

    private void OnReachedBase()
    {
        _movement.Reached -= OnReachedBase;
        ReachedBase?.Invoke(this);
    }
}

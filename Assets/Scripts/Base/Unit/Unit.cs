using UnityEngine;

[RequireComponent(typeof(UnitMovement), typeof(Collector))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    private Collector _collector;
    private UnitMovement _movement;
    [SerializeField] private Resource _targetResource;

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
        _movement.Reached += OnReached;
    }

    private void OnReached()
    {
        _movement.Reached -= OnReached;

        _collector.Take(_targetResource);

        MoveTo(_base.ArrivalPoint);
        _movement.Reached += OnReachedBase;
    }

    private void MoveTo(Transform target)
    {
        _movement.SetTarget(target);
        _movement.Move();
    }

    private void OnReachedBase()
    {
        _movement.Reached -= OnReachedBase;
        _collector.Put(_base, this);
    }
}

using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _maxTakeDistance;
    [SerializeField] private DistanceChecker _distanceChecker;

    private Resource _resource;

    public bool IsBusy => _resource != null;

    public void Take(Resource resource)
    {
        bool distancePassed = _distanceChecker.CheckDistance(transform.position, resource.transform.position, _maxTakeDistance);

        if (distancePassed == false || IsBusy)
            return;

        _resource = resource;
        _resource.transform.SetParent(transform);
        _resource.transform.position = _handPoint.position;

        _resource.Claim();
    }

    public void Put(Base @base, Unit unit)
    {
        @base.ClaimResource(_resource, unit);

        _resource.transform.SetParent(null);
        _resource = null;
    }
}
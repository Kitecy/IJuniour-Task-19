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
        bool distancePassed = _distanceChecker.IsDistanceGreater(transform.position, resource.transform.position, _maxTakeDistance);

        if (distancePassed == false || IsBusy)
            return;

        _resource = resource;
        _resource.transform.SetParent(transform);
        _resource.transform.position = _handPoint.position;

        _resource.Claim();
    }

    public Resource GiveResource()
    {
        Resource temp = _resource;
        _resource.transform.SetParent(null);
        _resource = null;
        return temp;
    }
}
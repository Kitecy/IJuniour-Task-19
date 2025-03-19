using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour, IHasArrivalPoint
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private List<ResourcePlace> _places;
    [SerializeField] private ResourceSpawner _spawner;

    private readonly List<Resource> _waitingList = new();

    private int _resources;

    [field: SerializeField] public Transform ArrivalPoint { get; private set; }

    private void OnEnable()
    {
        foreach (ResourcePlace place in _places)
            place.Spawned += OnResourceSpawned;
    }

    private void OnDisable()
    {
        foreach (ResourcePlace place in _places)
            place.Spawned -= OnResourceSpawned;
    }

    public void ClaimResource(Resource resource, Unit unit)
    {
        _units.Add(unit);

        _resources++;
        _spawner.ReleaseObject(resource);

        if (_waitingList.Count != 0)
            SendForResourceFromWaitingList();
    }

    private void OnResourceSpawned(Resource resource)
    {
        if (_units.Count == 0)
        {
            _waitingList.Add(resource);
            return;
        }

        SendForResource(resource);
    }

    private void SendForResource(Resource resource)
    {
        Unit unit = _units.First();
        _units.Remove(unit);

        unit.GoToResources(resource);
    }

    private void SendForResourceFromWaitingList()
    {
        if (_units.Count == 0 || _waitingList.Count == 0)
            return;

        Resource resource = _waitingList.First();
        _waitingList.Remove(resource);

        SendForResource(resource);
    }
}

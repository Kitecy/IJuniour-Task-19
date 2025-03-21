using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private List<ResourcePlace> _places;
    [SerializeField] private ResourceSpawner _spawner;

    private readonly List<Resource> _waitingList = new();

    private int _resources;

    public event Action<int> ResourcesChanged;

    [field: SerializeField] public Transform ArrivalPoint { get; private set; }

    private void OnEnable()
    {
        foreach (ResourcePlace place in _places)
            place.Spawned += OnResourceSpawned;

        foreach (Unit unit in _units)
        {
            unit.ReachedResource += OnUnitReachedResource;
            unit.ReachedBase += OnUnitReachedBase;
        }
    }

    private void OnDisable()
    {
        foreach (ResourcePlace place in _places)
            place.Spawned -= OnResourceSpawned;

        foreach (Unit unit in _units)
        {
            unit.ReachedResource -= OnUnitReachedResource;
            unit.ReachedBase -= OnUnitReachedBase;
        }
    }

    private void OnUnitReachedResource(Unit unit)
    {
        unit.GoToBase(ArrivalPoint);
    }

    private void OnUnitReachedBase(Unit unit)
    {
        _spawner.ReleaseObject(unit.GiveResource());

        _resources++;
        ResourcesChanged?.Invoke(_resources);

        _units.Add(unit);

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
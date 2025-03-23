using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private ResourcesQueue _resourceQueue;

    private int _resources;

    public event Action<int> ResourcesChanged;

    [field: SerializeField] public Transform ArrivalPoint { get; private set; }

    private void Awake()
    {
        _scanner.Scanned += OnResourcesScanned;
    }

    private void OnEnable()
    {
        foreach (Unit unit in _units)
        {
            unit.ReachedResource += OnUnitReachedResource;
            unit.ReachedBase += OnUnitReachedBase;
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _units)
        {
            unit.ReachedResource -= OnUnitReachedResource;
            unit.ReachedBase -= OnUnitReachedBase;
        }
    }

    private void OnResourcesScanned(List<Resource> resources)
    {
        if (resources.Count == 0 || _units.Count == 0)
            return;

        for (int i = 0; i < resources.Count && _units.Count > 0; i++)
            if (_resourceQueue.TryAddResource(resources[i]))
                SendForResource(resources[i]);
    }

    private void OnUnitReachedResource(Unit unit)
    {
        unit.GoToBase(ArrivalPoint);
    }

    private void OnUnitReachedBase(Unit unit)
    {
        Resource collectedResource = unit.GiveResource();
        _spawner.ReleaseObject(collectedResource);
        _resourceQueue.RemoveResource(collectedResource);

        _resources++;
        ResourcesChanged?.Invoke(_resources);

        _units.Add(unit);
    }

    private void SendForResource(Resource resource)
    {
        Unit unit = _units.First();
        _units.Remove(unit);

        unit.GoToResources(resource);
    }
}
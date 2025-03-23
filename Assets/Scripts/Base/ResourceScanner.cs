using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanDelay;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _detectableLayers;

    private WaitForSeconds _sleepTime;

    public event Action<List<Resource>> Scanned;

    private void Awake()
    {
        _sleepTime = new(_scanDelay);
    }

    private void Start()
    {
        Scan();
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, _radius);
    }

    private void Scan()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _detectableLayers);
        List<Resource> resources = new();

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);

        StartCoroutine(WaitForScan());

        Scanned?.Invoke(resources);
    }

    private IEnumerator WaitForScan()
    {
        yield return _sleepTime;
        Scan();
    }
}

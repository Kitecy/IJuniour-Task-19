using System.Collections.Generic;
using UnityEngine;

public class ResourcesQueue : MonoBehaviour
{
    private HashSet<Resource> _resources = new();

    public bool TryAddResource(Resource resource)
    {
        return _resources.Add(resource);
    }

    public void RemoveResource(Resource resource)
    {
        _resources.Remove(resource);
    }
}

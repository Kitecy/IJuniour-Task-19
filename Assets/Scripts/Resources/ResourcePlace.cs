using System;
using System.Collections;
using UnityEngine;

public class ResourcePlace : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private ResourceSpawner _spawner;

    private bool _isSpawning = true;
    private WaitForSeconds _sleepTime;

    public event Action<Resource> Spawned;

    public Resource Resource { get; private set; }
    public bool HaveResource => Resource != null;

    private void Awake()
    {
        _sleepTime = new(_spawnDelay);
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (HaveResource)
            return;

        Resource = _spawner.GetObject();
        Resource.transform.position = _spawnPosition.position;
        Resource.Claimed += OnResourceClaimed;

        Spawned?.Invoke(Resource);
    }

    private IEnumerator Spawning()
    {
        yield return _sleepTime;
        Spawn();
    }

    private void OnResourceClaimed()
    {
        Resource.Claimed -= OnResourceClaimed;
        StartCoroutine(Spawning());
        Resource = null;
    }
}
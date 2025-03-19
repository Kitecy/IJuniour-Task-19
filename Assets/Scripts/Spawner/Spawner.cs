using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<T> _pool;

    private void Awake()
    {
        _pool = new(CreateObject, OnGetObject, OnReleaseObject, defaultCapacity: _defaultCapacity, maxSize: _maxSize);
    }

    public T GetObject() =>
        _pool.Get();

    public void ReleaseObject(T obj) =>
        _pool.Release(obj);

    private T CreateObject()
    {
        return Instantiate(_prefab);
    }

    private void OnReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetObject(T obj)
    {
        obj.gameObject.SetActive(true);
    }
}

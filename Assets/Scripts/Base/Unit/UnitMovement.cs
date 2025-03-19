using System;
using System.Collections;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;
    private Coroutine _coroutine;

    public event Action Reached;

    private Vector3 ProcessedTargetPosition => new Vector3(_target.position.x, transform.position.y, _target.position.z);

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Move()
    {
        if (_coroutine == null && _target != null)
            _coroutine = StartCoroutine(Walking());
    }

    public bool GetReached()
    {
        return transform.position == ProcessedTargetPosition;
    }

    private IEnumerator Walking()
    {
        while (GetReached() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, ProcessedTargetPosition, _speed * Time.deltaTime);

            yield return null;
        }

        _coroutine = null;

        Reached?.Invoke();
    }
}

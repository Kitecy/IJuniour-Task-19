using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action Claimed;

    [field: SerializeField] public Transform ArrivalPoint { get; private set; }

    public void Claim()
    {
        Claimed?.Invoke();
    }
}

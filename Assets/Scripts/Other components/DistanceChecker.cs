using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public bool IsDistanceGreater(Vector3 a, Vector3 b, float distance)
    {
        return (a - b).sqrMagnitude < distance * distance;
    }
}
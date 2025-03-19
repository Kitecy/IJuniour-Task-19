using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public bool CheckDistance(Vector3 a, Vector3 b, float distance)
    {
        return (a - b).sqrMagnitude < distance;
    }
}
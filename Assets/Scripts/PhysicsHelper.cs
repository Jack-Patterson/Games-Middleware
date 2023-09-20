using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHelper
{
    internal static Vector3 GetParallelComponent(Vector3 vector, Vector3 normal)
    {
        float dotProduct = Vector3.Dot(vector, normal);
        Vector3 parallelComponent = dotProduct * normal;

        return parallelComponent;
    }

    internal static Vector3 GetPerpendicularComponent(Vector3 vector, Vector3 normal)
    {
        Vector3 perpendicularComponent = vector - GetParallelComponent(vector, normal);

        return perpendicularComponent;
    }

    internal static float GetDistance(Vector3 vectorToTarget, Vector3 targetTransformUp)
    {
        Vector3 distanceToTarget = GetParallelComponent(vectorToTarget, targetTransformUp);
        float distance = distanceToTarget.magnitude;

        return distance;
    }

    internal static Vector3 GetVelocity(Vector3 vector, Vector3 normal, float cor)
    {
        Vector3 velocity = GetPerpendicularComponent(vector, normal) -
            (cor * GetParallelComponent(vector, normal));

        return velocity;
    }

    internal static Vector3 GetVectorToTarget(Vector3 firstVector, Vector3 secondVector)
    {
        Vector3 vectorToTarget = firstVector - secondVector;

        return vectorToTarget;
    }
}

using UnityEngine;

public class PhysicsSphere : MonoBehaviour
{
    [SerializeField] private GameObject planeRef;
    private Vector3 velocity, acceleration;
    private const float gravityValue = 9.8f;
    private const float coeffecientOfRestitution = 1;
    
    void Start()
    {
        acceleration = gravityValue * Vector3.down;
    }

    void Update()
    {
        Vector3 vectorToTarget = GetVectorToTarget(transform.position, planeRef.transform.position);
        Vector3 normal = planeRef.transform.up;

        float distance = GetDistance(vectorToTarget, normal);
        float scale = transform.localScale.y / 2;

        velocity += acceleration * Time.deltaTime;

        Vector3 deltaS = velocity * Time.deltaTime;

        #region First Implementation
        /*
        transform.position += deltaS;
        
        if (distance <= scale)
        {
            transform.position += GetPerpendicularComponent(deltaS, normal) - GetParallelComponent(deltaS, normal);
            velocity = GetVelocity(velocity, normal, coeffecientOfRestitution);
        }
        */
        #endregion

        #region Second Implementation
        float d0 = distance - scale;

        transform.position += deltaS;

        Vector3 newVectorToTarget = GetVectorToTarget(transform.position, planeRef.transform.position);
        #endregion
    }

    private float GetDistance(Vector3 vectorToTarget, Vector3 targetTransformUp)
    {
        Vector3 distanceToTarget = GetParallelComponent(vectorToTarget, targetTransformUp);
        float distance = distanceToTarget.magnitude;

        return distance;
    }

    private Vector3 GetVelocity(Vector3 vector, Vector3 normal, float cor) {
        Vector3 velocity = GetPerpendicularComponent(vector, normal) -
            (cor * GetParallelComponent(vector, normal));

        return velocity;
    }

    private Vector3 GetParallelComponent(Vector3 vector, Vector3 normal)
    {
        float dotProduct = Vector3.Dot(vector, normal);
        Vector3 parallelComponent = dotProduct * normal;
        
        return parallelComponent;
    }

    private Vector3 GetPerpendicularComponent(Vector3 vector, Vector3 normal)
    {
        Vector3 perpendicularComponent = vector - GetParallelComponent(vector, normal);
        
        return perpendicularComponent;
    }

    private Vector3 GetVectorToTarget(Vector3 firstVector, Vector3 secondVector)
    {
        Vector3 vectorToTarget = firstVector - secondVector;

        return vectorToTarget;
    }
}

using System;
using UnityEngine;

public class PhysicsSphere : MonoBehaviour
{
    [SerializeField] private PhysicsSphere targetRef;
    [SerializeField] internal Vector3 velocity;
    private Vector3 acceleration;
    private const float gravityValue = 9.8f;
    private const float coeffecientOfRestitution = 1;
    [SerializeField] private float assignRadius;
    private float mass = 1;

    public float Radius { get { return transform.localScale.x/2f; } private set { transform.localScale = 2 * value * Vector3.one; } }

    void Start()
    {
        acceleration = gravityValue * Vector3.down;
        Radius = assignRadius;
    }

    void Update()
    {
        Vector3 vectorToTarget = GetVectorToTarget(transform.position, targetRef.transform.position);
        Vector3 normal = targetRef.transform.up;

        float distance = GetDistance(vectorToTarget, normal);
        float scale = transform.localScale.y / 2;

        velocity += acceleration * Time.deltaTime;

        /*print(velocity);
        
        Vector3 deltaS = velocity * Time.deltaTime;

        #region First Implementation
        
        transform.position += deltaS;
        
        if (distance <= scale)
        {
            transform.position += GetPerpendicularComponent(deltaS, normal) - GetParallelComponent(deltaS, normal);
            velocity = GetVelocity(velocity, normal, coeffecientOfRestitution);
        }
        
        #endregion

        #region Second Implementation
        //float d0 = distance - scale;

        //transform.position += deltaS;

        //Vector3 newVectorToTarget = GetVectorToTarget(transform.position, planeRef.transform.position);
        #endregion
        */

        distance = Vector3.Distance(transform.position, targetRef.transform.position);

        if (distance < Radius + targetRef.Radius)
        {
            print("Collision");
        }
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

    internal bool IsColliding(PhysicsSphere physicsSphere)
    {
        float distance = Vector3.Distance(transform.position, physicsSphere.transform.position);
        print("colliding");
        return distance < Radius + physicsSphere.Radius;
    }

    internal Vector3 ResolveVelocityOfOther(PhysicsSphere physicsSphere)
    {
        Vector3 normal = transform.position - physicsSphere.transform.position;
        
        Vector3 u1 = GetParallelComponent(velocity, normal);
        Vector3 u2 = GetParallelComponent(physicsSphere.velocity, normal);

        Vector3 s1 = GetPerpendicularComponent(velocity, normal);
        Vector3 s2 = GetPerpendicularComponent(physicsSphere.velocity, normal);

        float m1 = mass;
        float m2 = physicsSphere.mass;

        Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;
        Vector3 v2 = ((2 * m1) / (m1 + m2)) * u1 + ((m2 - m1) / (m1 + m2)) * u2;

        velocity = v1 + s1;
        Vector3 physicsSphereVelocity  = v2 + s2;

        return physicsSphereVelocity;
    }
}

using System;
using UnityEngine;

public class PhysicsSphere : MonoBehaviour, ICollidable
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float assignRadius;
    [SerializeField] private float mass = 1;
    private Vector3 acceleration;
    internal const float coeffecientOfRestitution = 1;

    public float Radius { get { return transform.localScale.x / 2f; } set { transform.localScale = 2 * value * Vector3.one; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

    void Start()
    {
        acceleration = PhysicsHelper.gravityValue * Vector3.down;
        Radius = assignRadius;
    }

    void Update()
    {
        HandleRegularVelocityMovement();      
    }

    public bool IsColliding(ICollidable collidableObject)
    {
        float distance;

        switch (collidableObject)
        {
            case PhysicsSphere ps:
                distance = Vector3.Distance(transform.position, collidableObject.Position);
                return distance < Radius + ps.Radius;
            case PhysicsPlane pp:
                Vector3 vectorToTarget = PhysicsHelper.GetVectorToTarget(transform.position, pp.transform.position);
                Vector3 normal = pp.transform.up;
                distance = PhysicsHelper.GetDistance(vectorToTarget, normal);
                
                return distance < Radius;
            default:
                return false;
        }
    }

    public (Vector3, Vector3) ResolveCollisionWithOther(ICollidable collidableObject)
    {
        switch (collidableObject)
        {
            case PhysicsSphere ps:
                return ResolveCollisionWithSphere(ps);
            case PhysicsPlane pp:
                ResolveCollisionWithPlane(pp);
                return (Vector3.zero, pp.Position);
            default:
                return (Vector3.zero, Vector3.zero);
        }
    }

    private (Vector3, Vector3) ResolveCollisionWithSphere(PhysicsSphere physicsSphere)
    {
        Vector3 thisDeltaS = velocity * Time.deltaTime;
        Vector3 thisPreviousPosition = transform.position - thisDeltaS;

        Vector3 otherDeltaS = physicsSphere.velocity * Time.deltaTime;
        Vector3 otherPreviousPosition = physicsSphere.Position - otherDeltaS;

        float d0 = Vector3.Distance(thisPreviousPosition, otherPreviousPosition) - Radius - physicsSphere.Radius;
        float d1 = Vector3.Distance(transform.position, physicsSphere.Position) - Radius - physicsSphere.Radius;

        float timeOfImpact = d1 * (Time.deltaTime / (d0 - d1));
        transform.position -= Velocity * (Time.deltaTime + timeOfImpact);
        physicsSphere.Position -= physicsSphere.Velocity * (Time.deltaTime + timeOfImpact);

        Vector3 normal = (transform.position - physicsSphere.transform.position).normalized;

        Vector3 u1 = PhysicsHelper.GetParallelComponent(velocity, normal);
        Vector3 u2 = PhysicsHelper.GetParallelComponent(physicsSphere.velocity, normal);

        Vector3 s1 = PhysicsHelper.GetPerpendicularComponent(velocity, normal);
        Vector3 s2 = PhysicsHelper.GetPerpendicularComponent(physicsSphere.velocity, normal);

        float m1 = mass;
        float m2 = physicsSphere.mass;

        Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;
        Vector3 v2 = ((2 * m1) / (m1 + m2)) * u1 + ((m2 - m1) / (m1 + m2)) * u2;

        velocity = v1 + s1;
        Vector3 physicsSphereVelocity = v2 + s2;

        transform.position += Velocity * (Time.deltaTime + timeOfImpact);
        Vector3 physicsSpherePosition = physicsSphere.Position + physicsSphereVelocity * (Time.deltaTime + timeOfImpact);

        return (physicsSphereVelocity, physicsSpherePosition);
    }

    private void ResolveCollisionWithPlane(PhysicsPlane physicsPlane)
    {
        Vector3 normal = physicsPlane.transform.up;
        Vector3 deltaS = velocity * Time.deltaTime;
        
        transform.position += PhysicsHelper.GetPerpendicularComponent(deltaS, normal) - PhysicsHelper.GetParallelComponent(deltaS, normal);
        velocity = PhysicsHelper.GetVelocity(velocity, normal, coeffecientOfRestitution);
    }

    private void HandleRegularVelocityMovement()
    {
        velocity += acceleration * Time.deltaTime;

        Vector3 deltaS = velocity * Time.deltaTime;

        transform.position += deltaS;
    }
}

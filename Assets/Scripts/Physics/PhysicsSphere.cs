using System;
using UnityEngine;

public class PhysicsSphere : MonoBehaviour, ICollidable
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float assignRadius;
    [SerializeField] private float mass = 1;
    [SerializeField] private float coeffecientOfRestitution = 0.75f;
    private Vector3 acceleration;
    private Vector3 previousPosition;

    private float Radius { get { return transform.localScale.x / 2f; } set { transform.localScale = 2 * value * Vector3.one; } }
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

    private (Vector3, Vector3) ResolveCollisionWithSphere(PhysicsSphere otherSphere)
    {
        Vector3 physicsSpherePosition, physicsSphereVelocity, toiThisPosition, 
            toiOtherPosition, toiThisVelocity, toiOtherVelocity;

        float d0 = Vector3.Distance(previousPosition, otherSphere.previousPosition) - Radius - otherSphere.Radius;
        float d1 = Vector3.Distance(transform.position, otherSphere.Position) - Radius - otherSphere.Radius;

        float timeOfImpact = d1 * (Time.deltaTime / (d1 - d0));

        toiThisPosition = transform.position - Velocity * timeOfImpact;
        toiOtherPosition = otherSphere.Position - otherSphere.Velocity * timeOfImpact;

        toiThisVelocity = Velocity - acceleration * timeOfImpact;
        toiOtherVelocity = otherSphere.Velocity - otherSphere.acceleration * timeOfImpact;
        
        Vector3 normal = (toiThisPosition - toiOtherPosition).normalized;

        Vector3 u1 = PhysicsHelper.GetParallelComponent(toiThisVelocity, normal);
        Vector3 u2 = PhysicsHelper.GetParallelComponent(toiOtherVelocity, normal);

        Vector3 s1 = PhysicsHelper.GetPerpendicularComponent(toiThisVelocity, normal);
        Vector3 s2 = PhysicsHelper.GetPerpendicularComponent(toiOtherVelocity, normal);

        float m1 = mass;
        float m2 = otherSphere.mass;

        Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;
        Vector3 v2 = ((2 * m1) / (m1 + m2)) * u1 + ((m2 - m1) / (m1 + m2)) * u2;

        v1 *= coeffecientOfRestitution;
        v2 *= otherSphere.coeffecientOfRestitution;

        v1 += acceleration * timeOfImpact;
        v2 += otherSphere.acceleration * timeOfImpact;

        Velocity = v1 + s1;
        physicsSphereVelocity = v2 + s2;

        transform.position = toiThisPosition + Velocity * timeOfImpact;
        physicsSpherePosition = toiOtherPosition + physicsSphereVelocity * timeOfImpact;

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
        previousPosition = transform.position;

        velocity += acceleration * Time.deltaTime;
        Vector3 deltaS = velocity * Time.deltaTime;
        transform.position += deltaS;
    }
}

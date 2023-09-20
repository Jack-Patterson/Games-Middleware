using System;
using UnityEngine;

public class PhysicsSphere : MonoBehaviour, ICollidable
{
    [SerializeField] private PhysicsSphere targetRef;
    [SerializeField] internal Vector3 velocity;
    [SerializeField] private float assignRadius;
    [SerializeField] private float mass = 1;
    private Vector3 acceleration;
    private const float gravityValue = 9.8f;
    private const float coeffecientOfRestitution = 1;

    public float Radius { get { return transform.localScale.x / 2f; } set { transform.localScale = 2 * value * Vector3.one; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    void Start()
    {
        acceleration = gravityValue * Vector3.down;
        Radius = assignRadius;
    }

    void Update()
    {
        Vector3 vectorToTarget = PhysicsHelper.GetVectorToTarget(transform.position, targetRef.transform.position);
        Vector3 normal = targetRef.transform.up;

        float distance = PhysicsHelper.GetDistance(vectorToTarget, normal);
        float scale = transform.localScale.y / 2;

        velocity += acceleration * Time.deltaTime;

        //print(velocity);
        
        Vector3 deltaS = velocity * Time.deltaTime;

        transform.position += deltaS;

        #region First Implementation
        

        //if (distance <= scale)
        //{
         //   transform.position += PhysicsHelper.GetPerpendicularComponent(deltaS, normal) - PhysicsHelper.GetParallelComponent(deltaS, normal);
         //   velocity = PhysicsHelper.GetVelocity(velocity, normal, coeffecientOfRestitution);
        //}
        
        #endregion
        
    }

    public bool IsColliding(ICollidable collidableObject)
    {
        float distance = Vector3.Distance(transform.position, collidableObject.GetPosition());

        switch (collidableObject)
        {
            case PhysicsSphere ps:
                return distance < Radius + ps.Radius;
            case PhysicsPlane pp:
                return distance < Radius;
            default:
                return false;
        }
    }

    public Vector3 ResolveCollisionWithOther(ICollidable collidableObject)
    {
        switch (collidableObject)
        {
            case PhysicsSphere ps:
                return ResolveCollisionWithSphere(ps);
            case PhysicsPlane pp:
                ResolveCollisionWithPlane(pp);
                return Vector3.zero;
            default:
                return Vector3.zero;
        }
    }

    private Vector3 ResolveCollisionWithSphere(PhysicsSphere physicsSphere)
    {
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

        return physicsSphereVelocity;
    }

    private void ResolveCollisionWithPlane(PhysicsPlane physicsPlane)
    {
        Vector3 normal = physicsPlane.transform.up;
        Vector3 deltaS = velocity * Time.deltaTime;

        transform.position += PhysicsHelper.GetPerpendicularComponent(deltaS, normal) - PhysicsHelper.GetParallelComponent(deltaS, normal);
        velocity = PhysicsHelper.GetVelocity(velocity, normal, coeffecientOfRestitution);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

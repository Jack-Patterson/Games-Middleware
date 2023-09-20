using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlane : MonoBehaviour, ICollidable
{
    public Vector3 Velocity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsColliding(ICollidable collidableObject)
    {
        float distance = Vector3.Distance(transform.position, collidableObject.GetPosition());

        switch (collidableObject)
        {
            case PhysicsSphere ps:
                return distance <= ps.Radius;
            default:
                return false;
        }
    }

    public Vector3 ResolveCollisionWithOther(ICollidable collidableObject)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.up = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

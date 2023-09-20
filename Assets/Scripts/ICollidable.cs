using UnityEngine;

public interface ICollidable
{
    Vector3 Velocity { get; set; }

    public bool IsColliding(ICollidable collidableObject);
    public Vector3 ResolveCollisionWithOther(ICollidable collidableObject);
    public Vector3 GetPosition();
}

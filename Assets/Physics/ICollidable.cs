using UnityEngine;

public interface ICollidable
{
    Vector3 Velocity { get; set; }
    Vector3 Position { get; set; }

    public bool IsColliding(ICollidable collidableObject);
    public (Vector3, Vector3) ResolveCollisionWithOther(ICollidable collidableObject);
}

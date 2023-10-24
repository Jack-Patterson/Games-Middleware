using UnityEngine;

namespace Physics.Scripts
{
    public class PhysicsPlane : MonoBehaviour, ICollidable
    {
        // used in the demo scene I made
        [SerializeField] internal bool isKeepingScore = false;
        public Vector3 Velocity { get { return Vector3.zero; } set { } }
        public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

        public bool IsColliding(ICollidable collidableObject)
        {
            switch (collidableObject)
            {
                default:
                    return false;
            }
        }

        public (Vector3, Vector3) ResolveCollisionWithOther(ICollidable collidableObject)
        {
            switch (collidableObject)
            {
                default:
                    return (Vector3.zero, Vector3.zero);
            }
        }
    }
}

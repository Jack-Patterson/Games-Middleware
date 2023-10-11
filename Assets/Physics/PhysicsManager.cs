using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    private List<ICollidable> physicsObjects;
    
    void Start()
    {
        physicsObjects = FindObjectsOfType<MonoBehaviour>().OfType<ICollidable>().ToList();
    }

    void LateUpdate()
    {
        for (int i = 0; i < physicsObjects.Count-1; i++)
        {
            for (int j = i+1; j < physicsObjects.Count; j++)
            {
                ICollidable firstPhysicsObject = physicsObjects[i];
                ICollidable secondPhysicsObject = physicsObjects[j];

                if (SwitchObjectsAccordingToHierarchy(firstPhysicsObject, secondPhysicsObject))
                {
                    (firstPhysicsObject, secondPhysicsObject) = (secondPhysicsObject, firstPhysicsObject);
                }

                if (firstPhysicsObject.IsColliding(secondPhysicsObject))
                {
                    (secondPhysicsObject.Velocity, secondPhysicsObject.Position) = firstPhysicsObject.ResolveCollisionWithOther(secondPhysicsObject);
                }
            }
        }
    }

    private bool SwitchObjectsAccordingToHierarchy(ICollidable firstPhysicsObject, ICollidable secondPhysicsObject)
    {
        int[] hierarchyValues = new int[2];

        for (int i = 0; i < 2; i++)
        {
            ICollidable collidableObject = i == 0 ? firstPhysicsObject : secondPhysicsObject;
            
            switch (collidableObject)
            {
                case PhysicsSphere:
                    hierarchyValues[i] = 0;
                    break;
                case PhysicsPlane:
                    hierarchyValues[i] = 1;
                    break;
                default:
                    hierarchyValues[i] = int.MaxValue;
                    break;
            }
        }

        return hierarchyValues[0] > hierarchyValues[1];
    }
}

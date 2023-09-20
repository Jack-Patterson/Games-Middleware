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

    void Update()
    {
        for (int i = 0; i < physicsObjects.Count-1; i++)
        {
            for (int j = i+1; j < physicsObjects.Count; j++)
            {
                if (physicsObjects[i].IsColliding(physicsObjects[j]))
                {
                    print("colliding");
                    physicsObjects[j].Velocity = physicsObjects[i].ResolveCollisionWithOther(physicsObjects[j]);
                }
            }
        }
    }
}

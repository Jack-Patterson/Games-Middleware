using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    List<PhysicsSphere> physicsSpheres;
    
    void Start()
    {
        physicsSpheres = FindObjectsOfType<PhysicsSphere>().ToList();
    }

    void Update()
    {
        for (int i = 0; i < physicsSpheres.Count-1; i++)
        {
            for (int j = i+1; j < physicsSpheres.Count; j++)
            {
                if (physicsSpheres[i].IsColliding(physicsSpheres[j]))
                {
                    physicsSpheres[j].velocity = physicsSpheres[i].ResolveVelocityOfOther(physicsSpheres[j]);
                }
            }
        }
    }
}

using System;
using Tobii.Gaming;
using UnityEngine;

namespace EyeTracking
{
    public class EyeTracking : MonoBehaviour
    {
        void Update()
        {
            // Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
            //
            // Ray ray = Camera.main.ScreenPointToRay(gazePoint);
            // RaycastHit hit;
            // if (UnityEngine.Physics.Raycast(ray, out hit))
            // {
            //     print((hit.point, hit.collider.gameObject.name));
            // }
        }
    }
}

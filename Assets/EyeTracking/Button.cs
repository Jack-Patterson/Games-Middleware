using System;
using UnityEngine;

namespace EyeTracking
{
    public class Button : MonoBehaviour
    {
        private void Start()
        {
            EyeTrackingManager.Instance.Button = this;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            EyeTrackingManager.Instance.Door.OpenDoor();
        }
    }
}

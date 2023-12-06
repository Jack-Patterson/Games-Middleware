using System;
using UnityEngine;

namespace EyeTracking
{
    public class DoorButton : MonoBehaviour
    {
        // private void O
        // {
        //     
        // }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.GetComponentInChildren<EyeTracking>() != null)
            {
                EyeTrackingManager.Instance.InvokeDoorOpenEvent();
            }
        }
    }
}

using System;
using UnityEngine;

namespace EyeTracking
{
    public class EyeTrackingManager : MonoBehaviour
    {
        public static EyeTrackingManager Instance;
        internal event Action DoorOpenEvent;

        internal void InvokeDoorOpenEvent()
        {
            DoorOpenEvent?.Invoke();
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}

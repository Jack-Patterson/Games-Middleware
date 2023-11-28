using UnityEngine;

namespace EyeTracking
{
    public class EyeTrackingManager : MonoBehaviour
    {
        public static EyeTrackingManager Instance;
        internal Door Door { get; set; }
        internal Button Button { get; set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}

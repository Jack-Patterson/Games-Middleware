using System;
using Tobii.Gaming;
using UnityEngine;

namespace EyeTracking
{
    public class HeadRotate : MonoBehaviour
    {
        private void Update()
        {
            HeadPose headPose = TobiiAPI.GetHeadPose();
            print(headPose);
            print(headPose.Position);
            print(headPose.Rotation);
        }
    }
}

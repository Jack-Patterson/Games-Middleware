using System;
using UnityEngine;

namespace EyeTracking
{
    public class Door : MonoBehaviour
    {
        // 42 -10
        // 59
        private readonly Vector3 _targetPosition = new(59, 5.25f, -10);
        private const float MoveDuration = 2f;
        private Vector3 _initialPosition;
        private bool _openingDoor = false;
        private float _elapsedTime = 0f;

        private void Start()
        {
            EyeTrackingManager.Instance.DoorOpenEvent += OpenDoor;
            _initialPosition = transform.position;
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) OpenDoor();
            
            if (!_openingDoor) return;

            _elapsedTime += Time.deltaTime;
            
            float time = Mathf.Clamp01(_elapsedTime / MoveDuration);
            transform.position = Vector3.Lerp(_initialPosition, _targetPosition, time);

            if (time >= 1.0f) _openingDoor = false;
        }

        internal void OpenDoor()
        {
            _openingDoor = true;
        }
    }
}
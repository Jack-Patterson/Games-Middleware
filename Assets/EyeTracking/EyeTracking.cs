using System;
using Tobii.Gaming;
using UnityEngine;

namespace EyeTracking
{
    public class EyeTracking : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (TobiiAPI.GetUserPresence().IsUserPresent())
            {
                GazePoint gazePoint = TobiiAPI.GetGazePoint();
                if (gazePoint.IsValid)
                    RotateCamera(gazePoint.Screen);
            }

            Move();
        }

        private void Move()
        {
            Vector3 moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            transform.parent.Translate(moveDirection * (5f * Time.deltaTime));
            // _rigidbody.MovePosition(_rigidbody.position + (Input.GetAxis("Vertical") * 5f) * Time.fixedDeltaTime);
        }

        private void RotateCamera(Vector2 gazePoint)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 offset = gazePoint - screenCenter;
            Vector2 direction = offset.normalized;
            Vector3 movement = new Vector3(-direction.y, direction.x, 0) * (20f * Time.deltaTime);
            float rotationX = -direction.y * 20f * Time.deltaTime;
            float rotationY = direction.x * 20f * Time.deltaTime;
            print(rotationY);
            
            // transform.Rotate(new Vector3(0f, direction.x * 20f * Time.deltaTime, 0f));
            transform.parent.Rotate(new Vector3(0f, direction.x * 40f * Time.deltaTime, 0f));
            // transform.localRotation = Quaternion.Euler(0, rotationY, 0);
            
            // transform.parent.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}
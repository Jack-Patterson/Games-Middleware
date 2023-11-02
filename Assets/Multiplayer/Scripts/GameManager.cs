using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private List<CharacterController> _characterControllers;
        private bool _playerTargetsAssigned = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _characterControllers = new List<CharacterController>();
        }

        private void Update()
        {
            
            if (_playerTargetsAssigned == false && _characterControllers.Count == 2)
                SetPlayerTargets();
        }

        internal void CallCorrectHitEvent(ulong id)
        {
            foreach (CharacterController c in _characterControllers)
            {
                if (c.OwnerClientId == id)
                {
                    c.CallHitEvent();
                }
            }
        }

        private void SetPlayerTargets()
        {
            _characterControllers[0].SetPunchAimPosition(_characterControllers[1].CameraAimPosition);
            _characterControllers[1].SetPunchAimPosition(_characterControllers[0].CameraAimPosition);

            _playerTargetsAssigned = true;
        }

        internal void RegisterCharacter(CharacterController characterController)
        {
            _characterControllers.Add(characterController);
        }
    }
}

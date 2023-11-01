using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private List<CharacterController> _characterControllers;

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
            print(_characterControllers.Count);
        }

        internal void RegisterCharacter(CharacterController characterController)
        {
            _characterControllers.Add(characterController);
        }
    }
}

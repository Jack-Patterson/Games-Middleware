using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private List<CharacterController> _characterControllers;
        private List<ulong> _characterIds;
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
            _characterIds = new List<ulong>();
        }

        private void Update()
        {
            if (!_playerTargetsAssigned && _characterControllers.Count == 2)
                SetPlayerTargets();
        }

        internal CharacterController ObtainCorrectCharacterController(ulong id)
        {
            foreach (CharacterController character in _characterControllers)
            {
                if (character.OwnerClientId == id)
                {
                    return character;
                }
            }

            return null;
        }

        internal CharacterAnimatorController ObtainCorrectAnimationController(ulong id)
        {
            foreach (CharacterController character in _characterControllers)
            {
                CharacterAnimatorController animatorController = character.GetComponent<CharacterAnimatorController>();

                if (character.OwnerClientId == id)
                {
                    return animatorController;
                }
            }

            return null;
        }

        internal List<ulong> RetrieveOtherIds(ulong idToExclude)
        {
            List<ulong> idList = _characterIds;
            idList.Remove(idToExclude);

            return idList;
        }

        private void SetPlayerTargets()
        {
            _characterControllers[0].SetPunchAimPosition(_characterControllers[1].CameraAimPosition);
            _characterControllers[1].SetPunchAimPosition(_characterControllers[0].CameraAimPosition);

            _playerTargetsAssigned = true;
        }

        internal void RegisterCharacter(CharacterController characterController, ulong id)
        {
            _characterControllers.Add(characterController);
            _characterIds.Add(id);
        }
    }
}
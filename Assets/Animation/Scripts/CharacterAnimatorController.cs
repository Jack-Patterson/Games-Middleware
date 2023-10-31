using System;
using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterAnimatorController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            
            _characterController.MoveEvent += Move;
            _characterController.JumpEvent += Jump;
            _characterController.ToggleCombatEvent += ToggleCombat;
            _characterController.AttackEvent += Attack;
        }
        
        private void LateUpdate()
        {
            if (_characterController.DisableAllCharacterChanges || _characterController.DisableAllCharacterMovement)
            {
                _animator.SetBool(AnimatorConstants.AnimIsMoving, false);
            }
        }

        private void Move(float horizontalInput, float verticalInput, bool isShiftPressed)
        {
            _animator.SetBool(AnimatorConstants.AnimIsMoving, _characterController.IsMoving);
            _animator.SetBool(AnimatorConstants.AnimIsShiftPressed, isShiftPressed);
            _animator.SetFloat(AnimatorConstants.AnimAxisHorizontal, horizontalInput);
            _animator.SetFloat(AnimatorConstants.AnimAxisVertical, verticalInput);
        }

        private void Jump()
        {
            _animator.SetTrigger(AnimatorConstants.AnimJump);
        }
        
        private void ToggleCombat(bool isCombatReady)
        {
            _animator.SetBool(AnimatorConstants.AnimFightReady, isCombatReady);
        }

        private void Attack(bool shouldAttackAlternateHand)
        {
            _animator.SetTrigger(AnimatorConstants.AnimAttackTrigger);
            _animator.SetBool(AnimatorConstants.AnimAttackHand, shouldAttackAlternateHand);
        }
        
        private void OnDisable()
        {
            _characterController.MoveEvent -= Move;
            _characterController.JumpEvent -= Jump;
        }
    }
}

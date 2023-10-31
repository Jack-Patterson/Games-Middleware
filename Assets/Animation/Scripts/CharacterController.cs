using System;
using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    [RequireComponent(typeof(CharacterAnimatorController))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;

        internal bool DisableAllCharacterChanges { get; private set; }
        internal bool DisableAllCharacterMovement { get; private set; }
        internal bool DisableAllCharacterCombat { get; private set; }

        #region Fighting Variables

        internal bool IsCombatReady { get; private set; }
        internal bool ShouldAttackAlternateHand { get; private set; }

        #endregion

        #region Movement Variables

        private float _speedModifier = 1.5f;
        private const float JumpHeight = 150f;
        
        private const float BaseCharacterSpeed = 1.2f;
        private const float WalkModifier = 1f;
        private const float SprintModifier = 1.5f;

        internal bool IsMoving { get; private set; }

        internal bool IsShiftPressed { get; private set; }

        internal float HorizontalInput { get; private set; }

        internal float VerticalInput { get; private set; }

        #endregion

        internal event Action<float, float, bool> MoveEvent;
        internal event Action JumpEvent;
        internal event Action<bool> ToggleCombatEvent;
        internal event Action<bool> AttackEvent;
        internal event Action DisableMovementEvent;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            
            MoveEvent += Move;
            JumpEvent += Jump;
            ToggleCombatEvent += ToggleCombat;
            AttackEvent += Attack;
        }

        private void Update()
        {
            ResolveInputs();
        }

        private void Move(float horizontalInput, float verticalInput, bool isShiftPressed)
        {
            IsMoving = horizontalInput != 0 || verticalInput != 0;
            
            if (IsMoving)
            {
                Vector3 directionToMove = new(horizontalInput, transform.position.y, verticalInput);
                transform.Translate(directionToMove * (BaseCharacterSpeed * _speedModifier * Time.deltaTime));
            }
        }

        private void Jump()
        {
            if (DisableAllCharacterChanges || DisableAllCharacterMovement) return;
            _rigidbody.AddForce(Vector3.up * JumpHeight);
        }

        private void ToggleCombat(bool isCombatReady)
        {
            IsCombatReady = isCombatReady;
        }

        private void Attack(bool shouldAttackAlternateHand)
        {
            ShouldAttackAlternateHand = !shouldAttackAlternateHand;
            DisableAnimationsForPeriod(AnimatorConstants.AttackDuration);
        }

        private void ResolveInputs()
        {
            if (DisableAllCharacterChanges) return;
            
            float horizontalInput, verticalInput;
            bool isShiftPressed;
            
            (horizontalInput, verticalInput) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            (isShiftPressed, _speedModifier) = ResolveSprinting();
            
            MoveEvent?.Invoke(horizontalInput, verticalInput, isShiftPressed);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpEvent?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleCombatEvent?.Invoke(!IsCombatReady);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!IsCombatReady)
                {
                    ToggleCombatEvent?.Invoke(true);
                }
                else
                {
                    AttackEvent?.Invoke(ShouldAttackAlternateHand);
                }
            }
        }
        
        private (bool, float) ResolveSprinting()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                return (true, SprintModifier);
            }
            else
            {
                return (false, WalkModifier);
            }
        }

        private void DisableAnimationsForPeriod(float time) => StartCoroutine(WaitForTime(time));

        private IEnumerator WaitForTime(float time)
        {
            DisableAllCharacterChanges = true;
            yield return new WaitForSeconds(time);
            DisableAllCharacterChanges = false;
        }

        private void OnDisable()
        {
            MoveEvent -= Move;
            JumpEvent -= Jump;
        }
    }
}
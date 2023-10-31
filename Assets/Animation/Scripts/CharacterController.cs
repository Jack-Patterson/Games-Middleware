using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;
       
        private bool _disableCharacterChange;
        
        #region Fighting Variables
        private bool _fightReady, _punchLeft;
        #endregion
        
        #region Movement Variables
        private float _horizontalInput, _verticalInput;
        private bool _isMoving, _isShiftPressed, _isJumping;
        private float _speedModifier = 1.5f;
        private const float JumpHeight = 150f;
        private const float Speed = 1.2f;
        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleActualMovement();
            HandleAnimations();
        }

        private void HandleActualMovement()
        {
            (_horizontalInput, _verticalInput) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 directionToMove = new(_horizontalInput, transform.position.y, _verticalInput);

            if (directionToMove != new Vector3(0, transform.position.y, 0))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    _speedModifier = 1.5f;
                    _isShiftPressed = true;
                }
                else
                {
                    _speedModifier = 1f;
                    _isShiftPressed = false;
                }
                
                if (_disableCharacterChange) return;
                transform.Translate(directionToMove * (Speed * _speedModifier * Time.deltaTime));
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !_disableCharacterChange)
            {
                _rigidbody.AddForce(Vector3.up * JumpHeight);
                _isJumping = true;
            }
        }

        private void HandleAnimations()
        {
            if (_disableCharacterChange)
            {
                _animator.SetBool(AnimatorConstants.AnimMoving, false);
                return;
            }

            HandleMovement();
            HandleCombatAnimations();
        }

        private void HandleMovement()
        {
            _animator.SetBool(AnimatorConstants.AnimMoving, _isMoving);
            _animator.SetBool(AnimatorConstants.AnimShiftPressed, _isShiftPressed);
            _animator.SetFloat(AnimatorConstants.AnimAxisHorizontal, _horizontalInput);
            _animator.SetFloat(AnimatorConstants.AnimAxisVertical, _verticalInput);
            
            if (_isJumping)
            {
                _animator.SetTrigger(AnimatorConstants.AnimJump);
                _isJumping = false;
            }
        }

        private void HandleCombatAnimations()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleFightState();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_fightReady)
                {
                    _animator.SetBool(AnimatorConstants.AnimAttackLeft, _punchLeft);
                    _animator.SetTrigger(AnimatorConstants.AnimAttackTrigger);

                    _punchLeft = !_punchLeft;
                    DisableAnimationsForPeriod(0.5f);
                }
                else
                {
                    ToggleFightState();
                }
            }
        }

        private void ToggleFightState()
        {
            _fightReady = !_fightReady;
            _animator.SetBool(AnimatorConstants.AnimFightReady, _fightReady);
            DisableAnimationsForPeriod(0.6f);
        }

        private void DisableAnimationsForPeriod(float time) => StartCoroutine(WaitForTime(time));

        private IEnumerator WaitForTime(float time)
        {
            _disableCharacterChange = true;
            yield return new WaitForSeconds(time);
            _disableCharacterChange = false;
        }
    }
}
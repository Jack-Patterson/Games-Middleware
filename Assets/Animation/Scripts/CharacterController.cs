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
        
        #region Animation Variables
        private static readonly int AnimMoving = Animator.StringToHash("IsMoving");
        private static readonly int AnimShiftPressed = Animator.StringToHash("IsShiftPressed");
        private static readonly int AnimAttackLeft = Animator.StringToHash("PunchLeft");
        private static readonly int AnimAttackTrigger = Animator.StringToHash("Punch");
        private static readonly int AnimFightReady = Animator.StringToHash("FightReady");
        private static readonly int AnimAxisVertical = Animator.StringToHash("AxisVertical");
        private static readonly int AnimAxisHorizontal = Animator.StringToHash("AxisHorizontal");
        private static readonly int AnimJump = Animator.StringToHash("Jump");
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
                _animator.SetBool(AnimMoving, false);
                return;
            }

            HandleMovement();
            HandleCombatAnimations();
        }

        private void HandleMovement()
        {
            _animator.SetBool(AnimMoving, _isMoving);
            _animator.SetBool(AnimShiftPressed, _isShiftPressed);
            _animator.SetFloat(AnimAxisHorizontal, _horizontalInput);
            _animator.SetFloat(AnimAxisVertical, _verticalInput);
            
            if (_isJumping)
            {
                _animator.SetTrigger(AnimJump);
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
                    _animator.SetBool(AnimAttackLeft, _punchLeft);
                    _animator.SetTrigger(AnimAttackTrigger);

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
            _animator.SetBool(AnimFightReady, _fightReady);
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
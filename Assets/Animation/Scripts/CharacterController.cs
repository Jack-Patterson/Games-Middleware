using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;

        private bool _fightReady = false;
        private bool _isMoving = false;
        private bool _isShiftPressed = false;
        private bool _isJumping = false;
        private bool _disableAnimChange = false;
        
        private int _punchAmount = 0;
        private float _mouseHeldAmount = 0;
        
        private float _horizontalInput, _verticalInput;
        private readonly float _speed = 1.2f;
        private float _speedModifier = 1.5f;
        private float _jumpHeight = 150f;
        
        private static readonly int AnimMoving = Animator.StringToHash("IsMoving");
        private static readonly int AnimShiftPressed = Animator.StringToHash("IsShiftPressed");
        private static readonly int AnimPunchBlend = Animator.StringToHash("PunchBlend");
        private static readonly int AnimPunchTrigger = Animator.StringToHash("Punch");
        private static readonly int AnimFightReady = Animator.StringToHash("FightReady");
        private static readonly int AxisVertical = Animator.StringToHash("AxisVertical");
        private static readonly int AxisHorizontal = Animator.StringToHash("AxisHorizontal");

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
                
                transform.Translate(directionToMove * (_speed * _speedModifier * Time.deltaTime));
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * _jumpHeight);
                _isJumping = true;
            }
        }

        private void HandleAnimations()
        {
            if (_disableAnimChange) return;

            HandleMovement();
            HandleCombatAnimations();
        }

        private void HandleMovement()
        {
            _animator.SetBool(AnimMoving, _isMoving);
            _animator.SetBool(AnimShiftPressed, _isShiftPressed);
            _animator.SetFloat(AxisHorizontal, _horizontalInput);
            _animator.SetFloat(AxisVertical, _verticalInput);
            // _animator.SetTrigger(, _isJumping);
        }

        private void HandleCombatAnimations()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleFightState();
            }

            if (Input.GetMouseButtonUp(0) || _mouseHeldAmount >= 0.8f)
            {
                print(_mouseHeldAmount);

                if (_fightReady)
                {
                    _animator.SetFloat(AnimPunchBlend, _punchAmount);
                    _animator.SetTrigger(AnimPunchTrigger);

                    if (_punchAmount == 2)
                    {
                        _punchAmount = 0;
                        DisableAnimationsForPeriod(1.1f);
                    }
                    else
                    {
                        _punchAmount++;
                        DisableAnimationsForPeriod(1f);
                    }
                }
                else
                {
                    ToggleFightState();
                }

                _mouseHeldAmount = 0;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                _mouseHeldAmount += Time.deltaTime;
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
            _disableAnimChange = true;
            yield return new WaitForSeconds(time);
            _disableAnimChange = false;
        }
    }
}
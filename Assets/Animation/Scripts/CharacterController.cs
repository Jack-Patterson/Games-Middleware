using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;

        [SerializeField] private LayerMask layerToCheck;

        private const string AnimPunchBlend = "PunchBlend";
        private const string AnimPunchTrigger = "Punch";
        private const string AnimFightReady = "FightReady";
        private bool _fightReady = false;
        private int _punchAmount = 0;
        private float _mouseHeldAmount = 0;
        private bool _disableAnimChange = false;
        private float _horizontalInput, _verticalInput;
        private float _speed = 1.2f;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleMovement();
            HandleAnimations();
        }

        private void HandleMovement()
        {
            (_horizontalInput, _verticalInput) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 direction = new(_horizontalInput, transform.position.y, _verticalInput);
            transform.Translate(direction * (_speed * Time.deltaTime));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * 200f);
            }
        }

        private void HandleAnimations()
        {
            if (_disableAnimChange) return;

            HandleCombatAnimations();
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
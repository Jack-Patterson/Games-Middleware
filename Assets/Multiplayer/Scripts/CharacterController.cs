using System;
using System.Collections;
using UnityEngine;

namespace Multiplayer.Scripts
{
    [RequireComponent(typeof(CharacterAnimatorController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class CharacterController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Camera _camera;
        
        [field: SerializeField] internal Transform HandIkTarget { get; private set; }
        [field: SerializeField] internal Transform HandRef { get; private set; }
        internal bool DisableAllCharacterChanges { get; private set; }
        internal bool IsCombatReady { get; private set; }
        internal bool ShouldAttackAlternateHand { get; private set; }
        internal bool IsMoving { get; private set; }
        internal string PlayerId { get; private set; }

        [SerializeField] private Transform cameraAimPosition;
        [SerializeField] private Transform punchAimPosition;
        private float _speedModifier = 1.5f;
        private const float BaseCharacterSpeed = 1.2f;
        private const float WalkModifier = 1f;
        private const float SprintModifier = 2f;
        private const float JumpHeight = 150f;
        private Vector3 _cameraRotationOffset;

        internal event Action<float, float> RotateEvent;
        internal event Action<float, float, bool> MoveEvent;
        internal event Action JumpEvent;
        internal event Action<bool> ToggleCombatEvent;
        internal event Action<bool> AttackEvent;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = GetComponentInChildren<Camera>();
            
            PlayerId = Guid.NewGuid().ToString();

            DisableAllCharacterChanges = false;
            _cameraRotationOffset = _camera.transform.position - cameraAimPosition.position;
            GameManager.Instance.RegisterCharacter(this);

            RotateEvent += OnRotate;
            MoveEvent += OnMove;
            JumpEvent += OnJump;
            ToggleCombatEvent += OnToggleCombat;
            AttackEvent += OnAttack;
        }

        private void Update()
        {
            ResolveInputs();
        }

        private void ResolveInputs()
        {
            if (DisableAllCharacterChanges) return;

            float horizontalInput, verticalInput, mouseX, mouseY;
            bool isShiftPressed;

            (mouseX, mouseY) = (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            (horizontalInput, verticalInput) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            (isShiftPressed, _speedModifier) = ResolveSprinting();

            MoveEvent?.Invoke(horizontalInput, verticalInput, isShiftPressed);
            RotateEvent?.Invoke(mouseX, mouseY);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CheckIfTouchingGround())
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

        private void OnRotate(float mouseX, float mouseY)
        {
            float rotationSpeed = 1;
            Quaternion rotation;

            transform.Rotate(0, mouseX, 0);
            rotation = Quaternion.Euler(-mouseY * rotationSpeed, mouseX * rotationSpeed, 0);

            _cameraRotationOffset = rotation * _cameraRotationOffset;
            _camera.transform.position = cameraAimPosition.position + _cameraRotationOffset;
            _camera.transform.LookAt(cameraAimPosition.position);
        }

        private void OnMove(float horizontalInput, float verticalInput, bool isShiftPressed)
        {
            IsMoving = horizontalInput != 0 || verticalInput != 0;

            if (IsMoving)
            {
                Vector3 directionToMove = new(horizontalInput, transform.position.y, verticalInput);
                transform.Translate(directionToMove * (BaseCharacterSpeed * _speedModifier * Time.deltaTime));
            }
        }

        private void OnJump()
        {
            if (DisableAllCharacterChanges) return;
            _rigidbody.AddForce(Vector3.up * JumpHeight);
        }

        private void OnToggleCombat(bool isCombatReady)
        {
            IsCombatReady = isCombatReady;
        }

        private void OnAttack(bool shouldAttackAlternateHand)
        {
            ShouldAttackAlternateHand = !shouldAttackAlternateHand;
            HandIkTarget.position = punchAimPosition.position;
            DisableAnimationsForPeriod(Animation.Scripts.AnimatorConstants.AttackDuration);
        }

        private bool CheckIfTouchingGround()
        {
            return UnityEngine.Physics.Raycast(transform.position, Vector3.down, 0.2f);
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
            MoveEvent -= OnMove;
            JumpEvent -= OnJump;
        }
    }
}
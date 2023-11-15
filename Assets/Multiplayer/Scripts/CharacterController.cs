using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Scripts
{
    [RequireComponent(typeof(CharacterAnimatorController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class CharacterController : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        private Camera _camera;

        [field: SerializeField] internal Transform HandIkTarget { get; private set; }
        [field: SerializeField] internal Transform HandRef { get; private set; }
        [field: SerializeField] internal Transform PunchAimPosition { get; private set; }
        [field: SerializeField] internal Transform CameraAimPosition { get; private set; }
        [field: SerializeField] internal GameObject HostArmor { get; private set; }
        [field: SerializeField] internal int Health { get; private set; }
        internal readonly NetworkVariable<bool> DisableAllCharacterChanges = new NetworkVariable<bool>(false);

        private bool IsCombatReady { get; set; }
        private bool ShouldAttackAlternateHand { get; set; }
        internal bool IsMoving { get; private set; }
        internal string PlayerId { get; private set; }

        private float _speedModifier = 1.5f;
        private const float BaseCharacterSpeed = 1.2f;
        private const float WalkModifier = 1f;
        private const float SprintModifier = 2f;
        private const float JumpHeight = 50f;
        private Vector3 _cameraRotationOffset;
        private const int PunchDamage = 10;

        internal event Action<float, float> RotateEvent;
        internal event Action<float, float, bool> MoveEvent;
        internal event Action JumpEvent;
        internal event Action<bool> ToggleCombatEvent;
        internal event Action<bool> AttackEvent;
        internal event Action HitEvent;
        internal event Action DeathEvent;
        internal event Action<int> HealthEvent;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = GetComponentInChildren<Camera>();

            PlayerId = Guid.NewGuid().ToString();
            
            _cameraRotationOffset = _camera.transform.position - CameraAimPosition.position;

            GameManager.Instance.RegisterCharacter(this, OwnerClientId);
            if (!IsOwner) _camera.enabled = false;
            if ((IsHost && IsOwner) || (!IsHost && !IsOwner))
                HostArmor.SetActive(true);

            Health = 100;

            RotateEvent += OnRotate;
            MoveEvent += OnMove;
            JumpEvent += OnJump;
            ToggleCombatEvent += OnToggleCombat;
            AttackEvent += OnAttack;
            HitEvent += OnHit;
            DeathEvent += OnDeath;
            HealthEvent += OnHealth;
            
            if (IsOwner)
            {
                HealthEvent += UiManager.Instance.OnHealth;
                UiManager.Instance.SetInitialHealthText(Health.ToString("000"));
            }
        }

        private void Update()
        {
            if (!IsOwner) return;

            ResolveInputs();
        }

        private void ResolveInputs()
        {
            if (DisableAllCharacterChanges.Value) return;

            float horizontalInput, verticalInput, mouseX, mouseY;
            bool isShiftPressed;

            (mouseX, mouseY) = (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            (horizontalInput, verticalInput) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            (isShiftPressed, _speedModifier) = ResolveSprinting();

            MoveEvent?.Invoke(horizontalInput, verticalInput, isShiftPressed);
            RotateEvent?.Invoke(mouseX, mouseY);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpEvent?.Invoke();
                ServerRpcController.Instance.JumpServerRpc(OwnerClientId);
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
                    ServerRpcController.Instance.AttackServerRpc(OwnerClientId);
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
            _camera.transform.position = CameraAimPosition.position + _cameraRotationOffset;
            _camera.transform.LookAt(CameraAimPosition.position);
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
            if (DisableAllCharacterChanges.Value) return;
            _rigidbody.AddForce(Vector3.up * JumpHeight);
        }

        private void OnToggleCombat(bool isCombatReady)
        {
            IsCombatReady = isCombatReady;
        }

        private void OnAttack(bool shouldAttackAlternateHand)
        {
            ShouldAttackAlternateHand = !shouldAttackAlternateHand;
            HandIkTarget.position = PunchAimPosition.position;
            DisableAnimationsForPeriod(AnimatorConstants.AttackDuration);
        }

        private void OnPunch()
        {
            if (DisableAllCharacterChanges.Value) return;

            Collider[] colliders = UnityEngine.Physics.OverlapSphere(HandRef.position, 1f);

            foreach (Collider c in colliders)
            {
                CharacterController characterController = c.GetComponent<CharacterController>();

                if (characterController != null && c.gameObject != gameObject)
                {
                    characterController.HitEvent?.Invoke();
                }
            }
        }

        private void OnHit()
        {
            HealthEvent?.Invoke(PunchDamage);

            if (Health <= 0)
            {
                Health = 0;
                DeathEvent?.Invoke();
            }
        }

        private void OnDeath()
        {
            ServerRpcController.Instance.SetDisableAllCharacterChangesServerRpc(OwnerClientId, true);
        }

        private void OnHealth(int amount)
        {
            Health -= amount;
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

            return (false, WalkModifier);
        }

        internal void SetPunchAimPosition(Transform newTransform)
        {
            PunchAimPosition = newTransform;
        }

        internal void CallHitEvent()
        {
            HitEvent?.Invoke();
        }

        internal void CallJumpEvent()
        {
            JumpEvent?.Invoke();
        }
        
        internal void CallAttackEvent()
        {
            AttackEvent?.Invoke(ShouldAttackAlternateHand);
        }
        
        internal void CallDeathEvent()
        {
            DeathEvent?.Invoke();
        }

        internal void DisableAnimationsForPeriod(float time) => StartCoroutine(WaitForTime(time));

        private IEnumerator WaitForTime(float time)
        {
            ServerRpcController.Instance.SetDisableAllCharacterChangesServerRpc(OwnerClientId, true);
            yield return new WaitForSeconds(time);
            ServerRpcController.Instance.SetDisableAllCharacterChangesServerRpc(OwnerClientId, false);
        }

        private void OnDisable()
        {
            MoveEvent -= OnMove;
            JumpEvent -= OnJump;
        }
    }
}
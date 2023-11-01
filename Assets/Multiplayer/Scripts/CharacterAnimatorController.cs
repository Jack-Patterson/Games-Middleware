using UnityEngine;

namespace Multiplayer.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterAnimatorController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Animator _animator;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private ParticleSystem footParticles;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _audioSource = GetComponentInChildren<AudioSource>();

            _characterController.MoveEvent += OnMove;
            _characterController.JumpEvent += OnJump;
            _characterController.ToggleCombatEvent += OnToggleCombat;
            _characterController.AttackEvent += OnAttack;
        }

        private void LateUpdate()
        {
            if (_characterController.DisableAllCharacterChanges)
            {
                _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimIsMoving, false);
            }
        }

        private void OnMove(float horizontalInput, float verticalInput, bool isShiftPressed)
        {
            _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimIsMoving, _characterController.IsMoving);
            _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimIsShiftPressed, isShiftPressed);
            _animator.SetFloat(Animation.Scripts.AnimatorConstants.AnimAxisHorizontal, horizontalInput);
            _animator.SetFloat(Animation.Scripts.AnimatorConstants.AnimAxisVertical, verticalInput);
        }

        private void OnJump()
        {
            _animator.SetTrigger(Animation.Scripts.AnimatorConstants.AnimJump);
        }

        private void OnToggleCombat(bool isCombatReady)
        {
            _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimFightReady, isCombatReady);
        }

        private void OnAttack(bool shouldAttackAlternateHand)
        {
            _animator.SetTrigger(Animation.Scripts.AnimatorConstants.AnimAttackTrigger);
            _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimAttackHand, shouldAttackAlternateHand);
        }

        private void OnFootStep()
        {
            _audioSource.PlayOneShot(audioClip, Animation.Scripts.AnimatorConstants.FootStepsVolume);
            footParticles.Play();
        }

        private void OnLandGround()
        {
            _audioSource.PlayOneShot(audioClip, Animation.Scripts.AnimatorConstants.LandGroundVolume);
            footParticles.Play();
        }

        private void OnPunch()
        {
            Collider[] colliders = UnityEngine.Physics.OverlapSphere(_characterController.HandRef.position, 1f);
            
            foreach (Collider c in colliders)
            {
                CharacterController characterController = c.GetComponent<CharacterController>();
                
                if (characterController != null && c.gameObject != gameObject)
                {
                    print((_characterController.PlayerId, characterController.PlayerId));
                }
            }
        }

        private void OnDisable()
        {
            _characterController.MoveEvent -= OnMove;
            _characterController.JumpEvent -= OnJump;
        }
    }
}
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterAnimatorController : NetworkBehaviour
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
            _characterController.HitEvent += OnHit;
            _characterController.DeathEvent += OnDeath;
        }

        private void LateUpdate()
        {
            if (_characterController.DisableAllCharacterChanges.Value)
            {
                _animator.SetBool(Animation.Scripts.AnimatorConstants.AnimIsMoving, false);
            }
        }

        private void OnMove(float horizontalInput, float verticalInput, bool isShiftPressed)
        {
            _animator.SetBool(AnimatorConstants.AnimIsMoving, _characterController.IsMoving);
            _animator.SetBool(AnimatorConstants.AnimIsShiftPressed, isShiftPressed);
            _animator.SetFloat(AnimatorConstants.AnimAxisHorizontal, horizontalInput);
            _animator.SetFloat(AnimatorConstants.AnimAxisVertical, verticalInput);
        }

        private void OnJump()
        {
            _animator.SetTrigger(Animation.Scripts.AnimatorConstants.AnimJump);
        }

        private void OnToggleCombat(bool isCombatReady)
        {
            _animator.SetBool(AnimatorConstants.AnimFightReady, isCombatReady);
        }

        private void OnAttack(bool shouldAttackAlternateHand)
        {
            _animator.SetTrigger(AnimatorConstants.AnimAttackTrigger);
            _animator.SetBool(AnimatorConstants.AnimAttackHand, shouldAttackAlternateHand);
        }
        
        private void OnHit()
        {
            _animator.SetTrigger(AnimatorConstants.AnimGetHit);
        }

        private void OnDeath()
        {
            _animator.SetTrigger(AnimatorConstants.AnimDeath);
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

        private void OnDisable()
        {
            _characterController.MoveEvent -= OnMove;
            _characterController.JumpEvent -= OnJump;
        }
    }
}
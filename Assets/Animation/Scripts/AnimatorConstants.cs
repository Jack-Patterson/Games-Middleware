using UnityEngine;

namespace Animation.Scripts
{
    public static class AnimatorConstants
    {
        internal static readonly int AnimIsMoving = Animator.StringToHash("IsMoving");
        internal static readonly int AnimIsShiftPressed = Animator.StringToHash("IsShiftPressed");
        internal static readonly int AnimAttackHand = Animator.StringToHash("AlternateAttackHand");
        internal static readonly int AnimAttackTrigger = Animator.StringToHash("Attack");
        internal static readonly int AnimFightReady = Animator.StringToHash("FightReady");
        internal static readonly int AnimAxisVertical = Animator.StringToHash("AxisVertical");
        internal static readonly int AnimAxisHorizontal = Animator.StringToHash("AxisHorizontal");
        internal static readonly int AnimJump = Animator.StringToHash("Jump");

        internal static readonly float FightReadyDuration = 0.6f;
        internal static readonly float AttackDuration = 0.5f;
        internal static readonly float FootStepsVolume = 0.05f;
        internal static readonly float LandGroundVolume = 0.1f;
    }
}
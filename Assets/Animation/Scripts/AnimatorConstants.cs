using UnityEngine;

namespace Animation.Scripts
{
    public static class AnimatorConstants
    {
        internal static readonly int AnimMoving = Animator.StringToHash("IsMoving");
        internal static readonly int AnimShiftPressed = Animator.StringToHash("IsShiftPressed");
        internal static readonly int AnimAttackLeft = Animator.StringToHash("PunchLeft");
        internal static readonly int AnimAttackTrigger = Animator.StringToHash("Punch");
        internal static readonly int AnimFightReady = Animator.StringToHash("FightReady");
        internal static readonly int AnimAxisVertical = Animator.StringToHash("AxisVertical");
        internal static readonly int AnimAxisHorizontal = Animator.StringToHash("AxisHorizontal");
        internal static readonly int AnimJump = Animator.StringToHash("Jump");
    }
}
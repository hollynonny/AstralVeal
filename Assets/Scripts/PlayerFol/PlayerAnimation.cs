using UnityEngine;

namespace PlayerFol
{
    public class PlayerAnimation
    {
        private static readonly int XForceHash = Animator.StringToHash("XForce");
        private static readonly int YForceHash = Animator.StringToHash("YForce");
        private static readonly int IsDashingHash = Animator.StringToHash("IsDashing");
        
        private Animator _anim;
        
        public PlayerAnimation(Animator animator)
        {
            _anim = animator;
        }

        public void SetMoveAnimation(float xForce)
        {
            _anim.SetFloat(XForceHash, xForce);
        }

        public void SetJumpAnimation(float yForce)
        {
            _anim.SetFloat(YForceHash, yForce);
        }

        public void SetDashAnimation(bool isDashing)
        {
            _anim.SetBool(IsDashingHash, isDashing);
        }
    }
}

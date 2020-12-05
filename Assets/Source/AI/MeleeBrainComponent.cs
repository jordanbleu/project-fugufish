using Assets.Source.AI.Base;
using System;
using UnityEngine;

namespace Assets.Source.AI
{
    /// <summary>
    /// Add to an object to give the 
    /// </summary>
    public class MeleeBrainComponent : BrainComponentBase
    {
        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }


        public override void ComponentUpdate()
        {
            UpdateAnimator();
            base.ComponentUpdate();
        }

        private void UpdateAnimator()
        {
            animator.SetBool("is_grounded", IsGrounded);
        }

        public override float CalculateHorizontalMovement()
        {
            return 0f;
        }

        public override float CalculateVerticalMovement()
        {
            return 0f;
        }
    }
}

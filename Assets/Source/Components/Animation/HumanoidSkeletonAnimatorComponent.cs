using Spine.Unity;
using System;
using UnityEngine;

namespace Assets.Source.Components.Animation
{
    /// <summary>
    /// Abstracts away animator controller magic strings for common humanoid animator controllers
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(SkeletonMecanim))]
    public class HumanoidSkeletonAnimatorComponent : ComponentBase
    {
        private Animator animator;
        private SkeletonMecanim skeletonMecanim;

        public bool IsGrounded { get; set; }
        public bool IsClimbing { get; set; }
        public float HorizontalMoveSpeed { get; set; }
        public float VerticalMoveSpeed { get; set; } 

        /// <summary>
        /// If true the skeleton has been flipped and is facing to the left
        /// </summary>
        public bool SkeletonIsFlipped { get => (skeletonMecanim.Skeleton.ScaleX < 0); }

        public override void ComponentAwake()
        {
            skeletonMecanim = GetRequiredComponent<SkeletonMecanim>();
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // Face actor the direction they are walking
            // if facing left, flip skeleton
            var scale = Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);

            if (!IsClimbing)
            {
                // Note - if horizontal speed is zero, leave the skeleton facing where it is
                if (HorizontalMoveSpeed < 0)
                {
                    skeletonMecanim.Skeleton.ScaleX = -scale;
                }
                else if (HorizontalMoveSpeed > 0)
                {
                    skeletonMecanim.Skeleton.ScaleX = scale;
                }
            }

            animator.SetBool("is_grounded", IsGrounded);
            animator.SetBool("is_climbing", IsClimbing);
            animator.SetFloat("horizontal_movement_speed", HorizontalMoveSpeed);
            animator.SetFloat("vertical_movement_speed", VerticalMoveSpeed);
            base.ComponentUpdate();
        }

        public void Jump() => animator.SetTrigger("jump");
        
        public void GroundPound() => animator.SetTrigger("ground_pound");
        
        public void Uppercut() => animator.SetTrigger("uppercut");

        public void Attack() => animator.SetTrigger("attack");
    }
}

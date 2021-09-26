using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Animation
{
    public class FinalBossAnimatorComponent : ComponentBase
    {
        protected Animator animator;        
        private SkeletonMecanim skeletonMecanim;

        public float HorizontalMoveSpeed = 0;

        public bool IsDead = false;
        public bool IsStunned = false;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            skeletonMecanim = GetRequiredComponent<SkeletonMecanim>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // a hack because im lazy
            var hSpeed = (HorizontalMoveSpeed != 0f) ? 1 : 0;
            animator.SetBool("is_dead", IsDead);
            animator.SetBool("is_stunned", IsStunned);
            animator.SetInteger("horizontal_move_speed", hSpeed);

            // Face actor the direction they are walking
            // if facing left, flip skeleton
            var scale = Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);


            // Note - if horizontal speed is zero, leave the skeleton facing where it is
            if (HorizontalMoveSpeed < 0)
            {
                skeletonMecanim.Skeleton.ScaleX = -scale;
            }
            else if (HorizontalMoveSpeed > 0)
            {
                skeletonMecanim.Skeleton.ScaleX = scale;
            }
            

            base.ComponentUpdate();
        }

        public bool SkeletonIsFlipped { get => skeletonMecanim.Skeleton.ScaleX < 0; }

        /// <summary>
        /// Face either left or right, towards the position
        /// </summary>
        /// <param name="position"></param>
        public void FaceTowardsPosition(Vector2 position) => FaceTowardsPosition(position.x);

        /// <summary>
        /// Face either left or right, towards the position
        /// </summary>
        /// <param name="position"></param>
        public void FaceTowardsPosition(float x)
        {

            var scale = Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);

            if (x < transform.position.x)
            {
                skeletonMecanim.Skeleton.ScaleX = -scale;
            }
            else
            {
                skeletonMecanim.Skeleton.ScaleX = scale;
            }
        }

        /// <summary>
        /// The slowest attack.  Boss does three ground smashes.
        /// </summary>
        public void BodySmash() => animator.SetTrigger("body_smash");
        
        /// <summary>
        /// Slow attack.  Boss does an overhead smash with just his arms, then has to take a sec to breathe
        /// </summary>
        public void DownwardSmash () => animator.SetTrigger("downward_smash");
        
        /// <summary>
        /// A slow-ish overhead slice
        /// </summary>
        public void OverheadSlice() => animator.SetTrigger("overhead_slice");
        
        /// <summary>
        /// a medium speed lower slice
        /// </summary>
        public void SwordSlice() => animator.SetTrigger("sword_slice");

        public void StunSquish() => animator.SetTrigger("stunned_squish");

        /// <summary>
        /// A quick forward slash with a forward momentum
        /// </summary>
        public void ForwardSlash() => animator.SetTrigger("forward_slash");

        public void Damage() => animator.SetTrigger("damage");




    }
}

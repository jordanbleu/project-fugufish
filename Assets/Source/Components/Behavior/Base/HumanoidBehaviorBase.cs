using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Behavior.Base
{
    [RequireComponent(typeof(SkeletonMecanim), typeof(HumanoidSkeletonAnimatorComponent))]
    public class HumanoidBehaviorBase : CommonPhysicsComponent
    {
        [Header("Humanoid Behavior Characteristics")]
        [SerializeField]
        [Tooltip("How far ahead the actor can see closer obstacles")]
        protected float lookaheadDistanceNear = 0.25f;
        
        [SerializeField]
        [Tooltip("How far ahead the actor can see far obstacles")]
        protected float lookaheadDistanceFar = 3;

        [SerializeField]
        [Tooltip("How far below the center of the actor's 'feet' they will check for ground.")]
        protected float lookBelowDistance = 0.5f;

        [SerializeField]
        [Tooltip("How fast the actor moves via walking / running")]
        protected float moveSpeed = 6f;

        [SerializeField]
        [Tooltip("How high the actor can jump")]
        protected float jumpHeight = 16f;

        [SerializeField]
        [Tooltip("How high the actor flies up when performing an uppercut")]
        protected float upperCutHeight = 8f;

        [SerializeField]
        [Tooltip("How forceful the actor's dodges are")]
        protected float dodgeSpeed = 24f;

        [SerializeField]
        [Tooltip("How quickly the actor climbs up a ladder")]
        protected float climbSpeed = 3f;

        [SerializeField]
        [Tooltip("Drag player object here")]
        protected GameObject player;

        // Components
        protected HumanoidSkeletonAnimatorComponent animator;

        // Cache the last calculated raycasts when using the intelligent movement
        private RaycastHit2D closeGroundHit;
        private RaycastHit2D farGroundHit;
        private RaycastHit2D forwardHit;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<HumanoidSkeletonAnimatorComponent>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            UpdateAnimator();
            base.ComponentUpdate();
        }

        private void UpdateAnimator()
        {
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = FootVelocity.x;
            animator.VerticalMoveSpeed = FootVelocity.y;   
        }

        protected Vector2 MoveIntelligentlyTowards(Vector2 position, float precision) {
            
            // Figure out what direction we're facing based on our animator
            var direction = (animator.SkeletonIsFlipped) ? -1 : 1;
            
            if (transform.position.x.IsWithin(precision, position.x)) {
                // if we're in the air, and nothing is below us, force us to keep moving so we don't fall
                if (!IsGrounded && closeGroundHit.transform == null) {
                    return new Vector2(direction*moveSpeed,CurrentVelocity.y);
                }                
                // if we're close enough to the position we seek, set to zero (but maintain gravity)
                return new Vector2(0f, CurrentVelocity.y);                
            }

            // Tell the animator to face towards the position
            animator.FaceTowardsPosition(position);

            // Do a ray cast to see what is in front of us
            var footCenter = GetFeetCenter();

            // Check if there is floor in front of us
            closeGroundHit = Physics2D.Raycast(new Vector2(footCenter.x, footCenter.y - lookBelowDistance), new Vector2(direction, 0), lookaheadDistanceNear, groundLayers, -999, 999);

            // if there's a solid in front of us or we're falling
            if (closeGroundHit.transform != null || !IsGrounded)
            {
                if (transform.position.x < position.x)
                {
                    return new Vector2(moveSpeed, CurrentVelocity.y);                    
                }
                else if (transform.position.x > position.x)
                {
                    return new Vector2(-moveSpeed, CurrentVelocity.y);
                }
            }
            else if (IsGrounded)
            {
                // Perform secondary raycasts to see if we can jump to something
                farGroundHit = Physics2D.Raycast(new Vector2(footCenter.x, footCenter.y - lookBelowDistance), new Vector2(direction, 0), lookaheadDistanceFar, groundLayers, -999, 999);
                forwardHit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), lookaheadDistanceFar, groundLayers, -999, 999);

                //  If there's something we can jump to, and we are (close enough) to not jumping or falling
                if (forwardHit.transform == null && farGroundHit.transform != null && CurrentVelocity.y.IsWithin(0.5f,0))
                {
                    // Jump up in the air.  Next frame we will move towards the player because we're falling
                    animator.Jump();
                    // Apply force to the rigid body directly so that gravity pulls us down
                    AddRigidBodyForce(0, jumpHeight);
                }
            }

            // There is nothing we can do, so wait here sadly.
            return new Vector2(0, CurrentVelocity.y);
        }

        

        public override void DrawAdditionalGizmosSelected()
        {
            if (UnityUtils.Exists(collider2d))
            {
                var feetCenter = GetFeetCenter();

                // If we are facing left
                if (animator.SkeletonIsFlipped)
                {
                    // Close Raycast
                    Gizmos.color = UnityUtils.Color(200, 50, 200);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x - lookaheadDistanceNear, feetCenter.y - lookBelowDistance));
                    // Far Raycast
                    Gizmos.color = UnityUtils.Color(200, 25, 100);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x - lookaheadDistanceFar, feetCenter.y - lookBelowDistance));
                    // Forward Raycast
                    Gizmos.color = UnityUtils.Color(255, 100, 150);
                    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - lookaheadDistanceFar, transform.position.y));
                }
                else
                {
                    // Close Raycast
                    Gizmos.color = UnityUtils.Color(200, 50, 200);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x + lookaheadDistanceNear, feetCenter.y - lookBelowDistance));
                    // Far Raycast
                    Gizmos.color = UnityUtils.Color(200, 25, 100);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x + lookaheadDistanceFar, feetCenter.y - lookBelowDistance));
                    // Forward Raycast
                    Gizmos.color = UnityUtils.Color(255, 100, 150);
                    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + lookaheadDistanceFar, transform.position.y));
                }

                // Draw the raycast hits
                if (closeGroundHit.transform != null)
                {
                    Gizmos.color = UnityUtils.Color(200, 50, 200);
                    Gizmos.DrawWireSphere(closeGroundHit.point, 0.1f);
                }

                if (farGroundHit.transform != null)
                {
                    Gizmos.color = UnityUtils.Color(200, 25, 100);
                    Gizmos.DrawWireSphere(farGroundHit.point, 0.1f);
                }

                if (forwardHit.transform != null) {
                    Gizmos.color = UnityUtils.Color(255, 100, 150);
                    Gizmos.DrawWireSphere(forwardHit.point, 0.1f);
                }

            }
            base.DrawAdditionalGizmosSelected();
        }

    }
}

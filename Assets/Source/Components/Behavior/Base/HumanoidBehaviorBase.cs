using Assets.Editor.Attributes;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Util;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Behavior.Base
{
    [RequireComponent(typeof(SkeletonMecanim), typeof(HumanoidSkeletonAnimatorComponent))]
    public class HumanoidBehaviorBase : CommonPhysicsComponent
    {
        [Header("Ground Check Ray (Red Line)")]
        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if he's about to step into a hole")]
        protected Vector2 groundCheckAngle = new Vector2(3f, -1.75f);

        [SerializeField]
        [Tooltip("This is the distance of the ray the actor uses to detect if he's about to step into a hole")]
        protected float groundCheckDistance = 0.5f;

        [Header("Jump Across Ray (Orange Line)")]
        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if he can jump across a gap in the floor")]
        protected Vector2 jumpAcrossAngle = new Vector2(0.1f, -0.05f);

        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if he can jump across a gap")]
        protected float jumpAcrossDistance = 0.2f;

        [Header("Forward Ray (Yellow Line)")]
        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if something is in front of him")]
        protected Vector2 forwardCheckAngle = new Vector2(0.1f, -0.05f);

        [SerializeField]
        [Tooltip("This is the distance of the ray the actor uses to detect if something is in front of him")]
        protected float forwardCheckDistance = 3f;


        [Header("Top Ray (Green Line)")]
        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if he can jump up")]
        protected Vector2 jumpUpAngle = new Vector2(0.1f, 0.1f);

        [SerializeField]
        [Tooltip("This is the distance of the ray the actor uses to detect if he can jump across a gap")]
        protected float jumpUpDistance = 3f;

        [Header("Down Ray(Blue Line)")]
        [SerializeField]
        [Tooltip("This is the angle of the ray the actor uses to detect if he can jump down from a raised platform")]
        protected Vector2 jumpDownAngle = new Vector2(0.1f, -0.2f);

        [SerializeField]
        [Tooltip("This is the distance of the ray the actor uses to detect if he can jump across a gap")]
        protected float jumpDownDistance = 3f;


        [Header("Humanoid Behavior Characteristics")]
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
        private RaycastHit2D groundCheckHit;
        private RaycastHit2D jumpAcrossHit;
        private RaycastHit2D forwardCheckHit;
        private RaycastHit2D jumpUpHit;
        private RaycastHit2D jumpDownHit;
        
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

        protected Vector2 MoveIntelligentlyTowards(Vector2 position, float precision)
        {

            // Figure out what direction we're facing based on our animator
            var direction = (transform.position.x > position.x) ? -1 : 1;

            if (transform.position.x.IsWithin(precision, position.x))
            {
                // if we're in the air, and nothing is below us, force us to keep moving so we don't fall
                if (!IsGrounded && groundCheckHit.transform == null)
                {
                    return new Vector2(direction * moveSpeed, CurrentVelocity.y);
                }
                // if we're close enough to the position we seek, set to zero (but maintain gravity)
                return new Vector2(0f, CurrentVelocity.y);
            }

            // Tell the animator to face towards the position
            animator.FaceTowardsPosition(position);

            var footCenter = GetFeetCenter();

            // Checks if there is ground in front of us
            groundCheckHit = MorePhysics2D.RaycastOther(gameObject, CreateCloseGroundRay(direction, footCenter), groundCheckDistance, groundLayers, -999, 999);
            
            // Checks if there's something we can jump over to
            jumpAcrossHit = MorePhysics2D.RaycastOther(gameObject, CreateJumpAcrossRay(direction, footCenter), jumpAcrossDistance, groundLayers, -999, 999);

            // Checks if something is in front of us
            forwardCheckHit = MorePhysics2D.RaycastOther(gameObject, CreateForwardRay(direction), forwardCheckDistance, groundLayers, -999, 999);
            
            // Check if something is above us
            jumpUpHit = MorePhysics2D.RaycastOther(gameObject, CreateJumpUpRay(direction), jumpUpDistance, groundLayers, -999, 999);

            // Checks if there's something we can jump down to
            jumpDownHit = MorePhysics2D.RaycastOther(gameObject, CreateJumpDownRay(direction, footCenter), jumpDownDistance, groundLayers, -999, 999);

            // if there's a solid in front of us or we're falling
            if (groundCheckHit.transform != null || !IsGrounded)
            {
                // Nothing is in our way
                if (forwardCheckHit.transform == null)
                {
                    // Move towards the destination
                    return new Vector2(Mathf.Sign(direction) * moveSpeed, CurrentVelocity.y);
                }
                // Something is in our way but we can jump over it
                else if (jumpUpHit.transform == null && CurrentVelocity.y.IsWithin(0.1f, 0))
                {
                    // Jump straight up in the air (next frame will carry our velocity towards our destination)
                    animator.Jump();
                    AddRigidBodyForce(0, jumpHeight);
                    return new Vector2(0, CurrentVelocity.y);
                }
            }
            // if we are on the ground but there's a gap in front of us
            else if (groundCheckHit.transform == null && IsGrounded)
            {
                // There's some ground to jump over to
                if (jumpAcrossHit.transform != null && CurrentVelocity.y.IsWithin(0.1f, 0))
                {
                    // Jump straight up in the air (next frame will carry our velocity towards our destination)
                    animator.Jump();
                    AddRigidBodyForce(0, jumpHeight);
                    return new Vector2(0, CurrentVelocity.y);
                }
                // There's nothing to jump over to, but we can drop down to floor below us
                else if (jumpDownHit.transform != null)
                {
                    // Move towards the destination
                    return new Vector2(Mathf.Sign(direction) * moveSpeed, CurrentVelocity.y);
                }
            }

            // If we get here, the AI has no idea what to do so just wait
            return new Vector2(0, CurrentVelocity.y);


        }
        private Ray CreateCloseGroundRay(int direction, Vector2 footCenter) =>        
            new Ray(new Vector2(footCenter.x, footCenter.y), new Vector2(direction * groundCheckAngle.x, groundCheckAngle.y));
        private Ray CreateJumpAcrossRay(int direction, Vector2 footCenter) =>        
            new Ray(new Vector2(footCenter.x, footCenter.y), new Vector2(direction * jumpAcrossAngle.x, jumpAcrossAngle.y));
        
        private Ray CreateForwardRay(int direction) => 
            new Ray(transform.position, new Vector2(direction * forwardCheckAngle.x, forwardCheckAngle.y));
       
        private Ray CreateJumpUpRay(int direction) =>       
           new Ray(transform.position, new Vector2(direction *jumpUpAngle.x, jumpUpAngle.y));
        
        private Ray CreateJumpDownRay(int direction, Vector2 footCenter) =>        
            new Ray(new Vector2(footCenter.x, footCenter.y), new Vector2(direction * jumpDownAngle.x, jumpDownAngle.y));
        

        public override void DrawAdditionalGizmosSelected()
        {
            if (UnityUtils.Exists(collider2d))
            {
                var footCenter = GetFeetCenter();
                var direction = animator.SkeletonIsFlipped ? -1 : 1;

                // red is the ground 
                var groundCheckRay = CreateCloseGroundRay(direction, footCenter);
                Gizmos.color = UnityUtils.Color(255, 0, 0);
                Gizmos.DrawLine(footCenter, groundCheckRay.GetPoint(groundCheckDistance));
                if (groundCheckHit.transform != null) {
                    Gizmos.DrawSphere(groundCheckHit.point, 0.1f);
                }
                
                // orange is the far ground
                var jumpAcrossRay = CreateJumpAcrossRay(direction, footCenter);
                Gizmos.color = UnityUtils.Color(255, 128, 0);
                Gizmos.DrawLine(footCenter, jumpAcrossRay.GetPoint(jumpAcrossDistance));
                if (jumpAcrossHit.transform != null) {
                    Gizmos.DrawSphere(jumpAcrossHit.point, 0.1f);
                }

                // yellow is forward
                var forwardRay = CreateForwardRay(direction);
                Gizmos.color = UnityUtils.Color(255, 255, 0);
                Gizmos.DrawLine(transform.position, forwardRay.GetPoint(forwardCheckDistance));
                if (forwardCheckHit.transform != null) {
                    Gizmos.DrawSphere(forwardCheckHit.point, 0.1f);
                }

                // green is top  / jump up
                var jumpUpRay = CreateJumpUpRay(direction);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, jumpUpRay.GetPoint(jumpUpDistance));
                if (jumpUpHit.transform != null) {
                    Gizmos.DrawSphere(jumpUpHit.point, 0.1f);
                }

                // blue is jump down
                var jumpDownRay = CreateJumpDownRay(direction, footCenter);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(footCenter, jumpDownRay.GetPoint(jumpDownDistance));
                if (jumpDownHit.transform != null) {
                    Gizmos.DrawSphere(jumpDownHit.point, 0.1f);
                }
   
            }
            base.DrawAdditionalGizmosSelected();
        }

    }
}

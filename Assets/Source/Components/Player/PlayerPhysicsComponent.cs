using Assets.Source.Components.Actor;
using Assets.Source.Components.Hazards;
using Assets.Source.Components.Physics;
using Assets.Source.Components.Physics.Base;
using Assets.Source.Components.Platforming;
using Assets.Source.Input.Constants;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerPhysicsComponent : PhysicsComponentBase
    {

        [SerializeField]
        private float moveSpeed = 6f;

        [SerializeField]
        [Tooltip("How high player can jump.  You might need to tweak the object's mass as well")]
        private float jumpHeight = 16f;

        [SerializeField]
        private float dodgeMaxSpeed = 24f;

        [SerializeField]
        private float dodgeDeceleration = 1f;

        [SerializeField]
        private float climbingSpeed = 3f;

        [SerializeField]
        private List<GameObject> collidingTriggers = new List<GameObject>();

        [SerializeField]
        [Tooltip("Speed at which the player is propelled his own sword swings")]
        private float attackVelocity = 3f;

        // used to add additional velocity to the player, which gets normalized automatically
        private Vector2 arbitraryVelocity = Vector2.zero;

        // if true, player is climbing on a ladder
        private bool isClimbing = false;

        // current movement speeds
        private float horizontalMove = 0f;
        private float verticalMove = 0f;
        // if true, player has used uppercut and cant do so again until he lands
        private bool usedUppercut = false;

        public float SkeletonScale { get => skeletonMecanim.Skeleton.ScaleX; }

        // true when the player is actively able to cause damage
        private bool isDamageEnabled = false;
        public bool IsDamageEnabled { get => isDamageEnabled; }

        // is attacking is true if the player is in the swing animation
        // This adds forward momentum and prevents some movement
        private bool isAttacking = false;
        public bool IsAttacking { get => isAttacking; }

        // Current speed at which the player is dodging 
        private float currentDodgeVelocity = 0f;

        // Component References
        private Animator animator;
        private SkeletonMecanim skeletonMecanim;
        private ActorComponent actor;

        public override void ComponentAwake()
        {
            skeletonMecanim = GetRequiredComponent<SkeletonMecanim>();
            animator = GetRequiredComponent<Animator>();
            actor = GetRequiredComponent<ActorComponent>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            isClimbing = collidingTriggers.Any(tr => tr.GetComponent<LadderComponent>() != null);

            var externalForces = CalculateEnvironmentalForces();

            

            if (!isClimbing) 
            {
                if (IsGrounded) 
                {
                    if (Input.IsKeyPressed(InputConstants.K_JUMP))
                    {
                        animator.SetTrigger("jump");
                        AddForce(new Vector2(0, jumpHeight));
                    }
                }

                if (Input.IsKeyPressed(InputConstants.K_SWING_SWORD)) {

                    // If we are in the air and the player is holding "down", do a GRAND SLAM
                    if (Input.IsKeyHeld(InputConstants.K_MOVE_DOWN) && !IsGrounded && !IsAttacking)
                    {
                        animator.SetTrigger("ground_pound");
                    }
                    // if we are in the air and player holds "up", do an uppercut, but only once before the player lands
                    else if (Input.IsKeyHeld(InputConstants.K_MOVE_UP) && !IsGrounded && !IsAttacking && !usedUppercut)
                    {
                        animator.SetTrigger("uppercut");
                        AddForce(new Vector2(0, jumpHeight));
                        usedUppercut = true;
                    }
                    else
                    {
                        animator.SetTrigger("attack");
                    }

                }
            }

            if (Input.IsKeyPressed(InputConstants.K_DODGE_LEFT)) 
            {
                currentDodgeVelocity -= dodgeMaxSpeed;
            }

            if (Input.IsKeyPressed(InputConstants.K_DODGE_RIGHT))
            {
                currentDodgeVelocity += dodgeMaxSpeed;
            }

            // if attacking, propel in the direction of the current player's skeleton
            var currentAttackSpeed = (isDamageEnabled) ? (attackVelocity*Mathf.Sign(skeletonMecanim.Skeleton.ScaleX)) : 0f;

            var totalXVelocity = arbitraryVelocity.x + currentDodgeVelocity + externalForces.x + currentAttackSpeed;
            var totalYVelocity = arbitraryVelocity.y + externalForces.y;

            ExternalVelocity = new Vector2(totalXVelocity, totalYVelocity);

            StabilizeVelocities();
            UpdateAnimator();
            base.ComponentUpdate();
        }


        private void UpdateAnimator()
        {
            animator.SetBool("is_grounded", IsGrounded);

            animator.SetFloat("horizontal_movement_speed", horizontalMove);
            animator.SetFloat("vertical_movement_speed", verticalMove);
            animator.SetBool("is_climbing", isClimbing);

            // if facing left, flip skeleton
            var scale = Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);
            
            // If climbing keep the skeleton the same 
            if (!isClimbing)
            {
                if (horizontalMove < 0)
                {
                    skeletonMecanim.Skeleton.ScaleX = -scale;
                }
                else if (horizontalMove > 0)
                {
                    skeletonMecanim.Skeleton.ScaleX = scale;
                }
            }
        }

        private Vector2 CalculateEnvironmentalForces()
        {
            var xForce = 0f;
            var yForce = 0f;

            // Calculates the total environmental force from ForceComponents
            foreach (var trigger in collidingTriggers) {
                if (trigger.TryGetComponent<ForceComponent>(out var forceComponent)) {
                    if (forceComponent.IsGround || IsGrounded) 
                    {
                        xForce += forceComponent.Amount.x;
                        yForce += forceComponent.Amount.y;
                    }
                }
            }
            return new Vector2(xForce, yForce);
        }

        private void StabilizeVelocities()
        {
            currentDodgeVelocity = currentDodgeVelocity.Stabilize(dodgeDeceleration, 0);

            arbitraryVelocity = new Vector2(arbitraryVelocity.x.Stabilize(dodgeDeceleration, 0),
                                            arbitraryVelocity.y.Stabilize(dodgeDeceleration,0));
        }

        public override float CalculateHorizontalMovement()
        {
            var moveRight = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            var moveLeft = Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            
            horizontalMove = moveRight - moveLeft;

            // if we're attacking, cancel out movement
            if (isAttacking) {
                horizontalMove = 0;
            }

            return horizontalMove * moveSpeed;
        }

        // Combines the force from all currently colliding force components 
        public override float CalculateVerticalMovement()
        {
            if (isClimbing)
            {
                IsGravityEnabled = false;

                // if climbing we have to disable normal physics and do our own thing
                var moveUp = Input.GetAxisValue(InputConstants.K_MOVE_UP);
                var moveDown = Input.GetAxisValue(InputConstants.K_MOVE_DOWN);
                verticalMove = moveUp - moveDown;
                return (moveUp - moveDown) * climbingSpeed;
            }
            else 
            {
                IsGravityEnabled = true;
            }
            return GetVelocity().y;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collidingTriggers.Add(collision.gameObject);

            if (collision.TryGetComponent<HazardComponent>(out var hazard)) {
                ReactToHazard(hazard);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collidingTriggers.Remove(collision.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<HazardComponent>(out var hazard))
            {
                ReactToHazard(hazard);
            }
        }

        private void ReactToHazard(HazardComponent hazard)
        { 
            actor.DepleteHealth(hazard.DamageAmount);

            var forceX = hazard.Force.x;
            var forceY = hazard.Force.y;

            if (hazard.ForceDirectionAffectedByVelocity) {
                var xSign = Mathf.Sign(hazard.Velocity.x);
                var ySign = Mathf.Sign(hazard.Velocity.y);

                if (xSign == 0) { xSign = 1; }
                if (ySign == 0) { ySign = 1; }

                forceX *= xSign;
                forceY *= ySign;
            }

            // if we are on the left side, force pushes us left, and vice versa
            if (hazard.ForceDirectionAffectedByColliderPosition) {

                if (transform.position.x < hazard.gameObject.transform.position.x)
                {
                    forceX = -Mathf.Abs(forceX);
                }
                else {
                    forceX = Mathf.Abs(forceX);
                }

                if (transform.position.y < hazard.gameObject.transform.position.y)
                {
                    forceY = -Mathf.Abs(forceY);
                }
                else
                {
                    forceY = Mathf.Abs(forceY);
                }
            }

            hazard.OnPlayerHit();

            // Add to the external velocity for this frame
            arbitraryVelocity = new Vector2(arbitraryVelocity.x + forceX, arbitraryVelocity.y + forceY);

            
        }

        // Called from unity event
        public void OnLandOnGround() {
            usedUppercut = false;
        }

        #region triggered via animation timeline event
        public void OnAttackBegin() {
            isAttacking = true;
        }

        public void OnAttackEnd() {
            isAttacking = false;
        }

        public void OnDamageEnable() {
            isDamageEnabled = true;
        }

        public void OnDamageDisable()
        {
            isDamageEnabled = false;
        }

        #endregion
    }
}

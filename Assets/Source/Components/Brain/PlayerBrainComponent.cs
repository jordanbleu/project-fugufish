using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Platforming;
using Assets.Source.Input.Constants;
using Spine.Unity;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    [RequireComponent(typeof(SkeletonMecanim), typeof(HumanoidSkeletonAnimatorComponent))]
    public class PlayerBrainComponent : CommonPhysicsComponent
    {

        [SerializeField]
        [Header("Player Brain")]
        [Tooltip("How fast the player moves via walking / running")]
        private float moveSpeed = 6f;

        [SerializeField]
        [Tooltip("How high the player can jump")]
        private float jumpHeight = 16f;

        [SerializeField]
        [Tooltip("How high the player flies up when performing an uppercut")]
        private float upperCutHeight = 8f;

        [SerializeField]
        [Tooltip("How forceful the player's dodges are")]
        private float dodgeSpeed = 24f;

        [SerializeField]
        [Tooltip("How quickly the player climbs up a ladder")]
        private float climbSpeed = 3f;

        [SerializeField]
        [Tooltip("How much movement force the player's attacks cause")]
        private float attackVelocity = 3f;

        // Components
        private HumanoidSkeletonAnimatorComponent animator;
        private LevelCameraEffectorComponent cameraEffector;


        private bool isClimbing = false;
        // Whether the player used the uppercut attack during his jump
        private bool usedUppercut = false;
        
        public bool IsDamageEnabled { get; private set; }
        public bool IsAttacking { get; private set; }

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<HumanoidSkeletonAnimatorComponent>();
            cameraEffector = GetRequiredComponent<LevelCameraEffectorComponent>(GetRequiredObject("Level"));
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // If we are currently touching any ladder components, we are climbing.
            isClimbing = CollidingTriggers.Any(tr => tr.GetComponent<LadderComponent>() != null);

            if (!isClimbing)
            {

                IsGravityEnabled = true;

                if (IsGrounded)
                {
                    if (Input.IsKeyPressed(InputConstants.K_JUMP))
                    {
                        animator.Jump();
                        // Apply force to the rigid body directly so that gravity pulls us down
                        AddRigidBodyForce(0, jumpHeight);
                    }
                }

                if (Input.IsKeyPressed(InputConstants.K_SWING_SWORD))
                {
                    // Shake the camera
                    cameraEffector.SwingRight();

                    // If we are in the air and the player is holding "up", do a GRAND SLAM
                    if (Input.IsKeyHeld(InputConstants.K_MOVE_UP) && !IsGrounded && !IsAttacking)
                    {
                        animator.GroundPound();
                    }
                    // if we are in the air and player holds "down", do an uppercut, but only once before the player lands
                    else if (Input.IsKeyHeld(InputConstants.K_MOVE_DOWN) && !IsAttacking && !usedUppercut)
                    {
                        AddImpact(0, upperCutHeight);
                        animator.Uppercut();
                        // Signals that we've already used the uppercut during this jump. 
                        // This will be reset to false upon landing.
                        usedUppercut = true;
                    }
                    else
                    {
                        animator.Attack();
                    }
                }
            }
            else {
                // disable gravity if we're on a ladder
                IsGravityEnabled = false;
            }

            if (Input.IsKeyPressed(InputConstants.K_DODGE_LEFT))
            {
                AddImpact(-dodgeSpeed, 0);
            }
            
            if (Input.IsKeyPressed(InputConstants.K_DODGE_RIGHT)) {
                AddImpact(dodgeSpeed, 0);
            }

            // Tranlate user controls into the player's movements
            UpdateFootVelocity();
            UpdateAnimator();
            base.ComponentUpdate();
        }

        private void UpdateAnimator()
        {
            animator.IsClimbing = isClimbing;
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = FootVelocity.x;
            animator.VerticalMoveSpeed = FootVelocity.y;
        }

        private void UpdateFootVelocity()
        {
            var inputHorizontal = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT) - Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            var inputVertical = Input.GetAxisValue(InputConstants.K_MOVE_UP) - Input.GetAxisValue(InputConstants.K_MOVE_DOWN);

            if (IsAttacking)
            {
                // Freeze X Position
                // Maintain y velocity so Unity can do gravity stuff
                FootVelocity = new Vector2(0, CurrentVelocity.y);
            }
            else if (isClimbing)
            {
                // Gravity is disabled while climbing
                FootVelocity = new Vector2(0, inputVertical * climbSpeed);
            }
            else {
                // Move as usual but again maintain the current y velocity so unity can do its thing
                FootVelocity = new Vector2(inputHorizontal * moveSpeed, CurrentVelocity.y);
            }

        }

        // Called from unity event
        public void OnLandOnGround()
        {
            usedUppercut = false;
        }

        #region Animation Events - Triggered via Spine Animation
        // ****************************************************
        // ** These must be wired up via Unity's timeline ******
        // ****************************************************
        public void OnAttackBegin()
        {
            IsAttacking = true;
        }

        public void OnAttackEnd()
        {
            IsAttacking = false;
        }

        public void OnDamageEnable()
        {
            IsDamageEnabled = true;
        }

        public void OnDamageDisable()
        {
            IsDamageEnabled = false;
        }

        public void OnGroundPoundLanded()
        {
            cameraEffector.LargeImpact();
        }

        #endregion
    }
}

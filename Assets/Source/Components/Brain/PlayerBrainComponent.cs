using Assets.Source.Components.Actor;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Level;
using Assets.Source.Components.Platforming;
using Assets.Source.Components.Sound;
using Assets.Source.Components.UI;
using Assets.Source.Enums;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using Assets.Source.Scene;
using Spine.Unity;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    [RequireComponent(typeof(SkeletonMecanim), typeof(HumanoidSkeletonAnimatorComponent))]
    public class PlayerBrainComponent : CommonPhysicsComponent
    {
        [SerializeField]
        [Tooltip("If true, the player will start the scene tied up and escape (really only for the very first scene in the game lol")]
        private bool isTiedUp = false;

        [SerializeField]
        [Tooltip("If true, the player will be unable to move, and will continue to be idle")]
        private bool isMovementLocked = false;

        [SerializeField]
        [Tooltip("If true the player will be restricted to walking")]
        private bool forceWalk = false;

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
        [Tooltip("How much movement force the player's attacks cause (not yet used)")]
        private float attackVelocity = 3f;

        [SerializeField]
        [Header("Stamina Requirements")]
        private int dodgeStaminaRequired = 30;

        [SerializeField]
        private DeathScreenComponent deathScreen;


        [SerializeField]
        private BloodSampleHudDisplayComponent bloodSampleDisplay;

        // Components
        private HumanoidSkeletonAnimatorComponent animator;
        private LevelCameraEffectorComponent cameraEffector;
        private MeleeComponent meleeCollider;
        private ActorComponent actor;
        private FootstepAudioComponent footsteps;
        private PlayerSoundEffects sound;
        private LevelComponent levelComponent;

        // true if the player is climbing
        private bool isClimbing = false;
        
        // Whether the player used the uppercut attack during his jump
        private bool usedUppercut = false;
        public bool IsAttacking { get; private set; }
        public AttackTypes ActiveAttack { get; private set; }

        // if this is true, the player has died, and fallen to the ground. 
        // Yes it is a very specific boolean okay i get it.
        private bool isDeadAndHitGround = false;

        private bool isDeathScreenEnabled = true;

        public override void ComponentAwake()
        {
            var levelObj = GetRequiredObject("Level");
            actor = GetRequiredComponent<ActorComponent>();
            animator = GetRequiredComponent<HumanoidSkeletonAnimatorComponent>();
            cameraEffector = GetRequiredComponent<LevelCameraEffectorComponent>(levelObj);
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>();
            footsteps = GetRequiredComponent<FootstepAudioComponent>();
            sound = GetRequiredComponent<PlayerSoundEffects>();
            levelComponent = GetRequiredComponent<LevelComponent>(levelObj);


            if (isTiedUp) {
                animator.Tied();
            }

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {

            if (UnityUtils.Exists(bloodSampleDisplay) && Input.IsKeyPressed(InputConstants.K_SHOWHUD)) {
                bloodSampleDisplay.ShowHud();
            }

            // Constantly update the game data tracker with our last ground position
            if (IsGrounded) {
                GameDataTracker.LastGroundedPosition = transform.position;
            }

            if (actor.IsAlive() && !isTiedUp && !isMovementLocked)
            {
                // If we are currently touching any ladder components, we are climbing.
                isClimbing = CollidingTriggers.Any(tr => UnityUtils.Exists(tr) && tr.GetComponent<LadderComponent>() != null);

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

                    if (Input.IsKeyPressed(InputConstants.K_SWING_SWORD) && actor.TryDepleteStamina(dodgeStaminaRequired/2))
                    {

                        var stupid = Input.GetAxisValue(InputConstants.K_MOVE_UP);
                        var dumb = Input.GetAxisValue(InputConstants.K_MOVE_DOWN);

                        // If we are in the air and the player is holding "up", do a GRAND SLAM
                        if (IsInputMoveUp() && !IsGrounded && !IsAttacking)
                        {
                            sound.Swing2();
                            animator.GroundPound();
                        }
                        // if we are in the air and player holds "down", do an uppercut, but only once before the player lands
                        else if (IsInputMoveDown() && !IsAttacking && !usedUppercut)
                        {
                            AddRigidBodyForce(0, upperCutHeight);
                            sound.Swing1();
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
                else
                {
                    // disable gravity if we're on a ladder
                    IsGravityEnabled = false;
                }

                if (Input.IsKeyPressed(InputConstants.K_DODGE_LEFT) && actor.TryDepleteStamina(dodgeStaminaRequired))
                {
                    sound.Roll();
                    animator.FaceTowardsPosition(new Vector2(transform.position.x-2, 0));
                    animator.Dodge();
                    AddImpact(-dodgeSpeed, 0);
                }

                if (Input.IsKeyPressed(InputConstants.K_DODGE_RIGHT) && actor.TryDepleteStamina(dodgeStaminaRequired))
                {
                    sound.Roll();
                    animator.FaceTowardsPosition(new Vector2(transform.position.x + 2, 0));
                    animator.Dodge();
                    AddImpact(dodgeSpeed, 0);
                }

                // Translate user controls into the player's movements
                UpdateFootVelocity();
            }
            else if (!actor.IsAlive()) {

                if (isDeathScreenEnabled && !deathScreen.gameObject.activeSelf) {
                    UpdateDeathData();
                    deathScreen.gameObject.SetActive(true);
                }

                if (!isDeadAndHitGround)
                {
                    // Force Player to keep falling
                    FootVelocity = new Vector2(0, CurrentVelocity.y);

                    if (IsGrounded)
                    {
                        isDeadAndHitGround = true;
                    }
                }
                else {
                    // Player is dead, and fell to the ground.
                    // Disable all the physics stuff, so the dead body just stays where it was on screen.
                    if (collider2d is CapsuleCollider2D capsuleCollider)
                    {
                        capsuleCollider.size = new Vector2(0.01f, 0.01f);
                        capsuleCollider.offset = new Vector2(0f, -1f);
                    }
                    FootVelocity = Vector2.zero;
                }
            }

            UpdateAnimator();

            // tell the melee collider to face left if the player is facing left -_-
            meleeCollider.IsFlipped = animator.SkeletonIsFlipped;

            base.ComponentUpdate();
        }

        /// <summary>
        /// Updates the GameDataTracker with the last death location and whatnot
        /// </summary>
        private void UpdateDeathData()
        {
            GameDataTracker.LastGroundedDeathPosition = GameDataTracker.LastGroundedPosition;
            GameDataTracker.LastDeathFrameName = levelComponent.CurrentlyActiveFrame.name;
        }

        private void UpdateAnimator()
        {
            animator.IsDead = !actor.IsAlive();
            animator.IsClimbing = isClimbing;
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = FootVelocity.x;
            animator.VerticalMoveSpeed = FootVelocity.y;
        }

        private void UpdateFootVelocity()
        {
            // These will be a value between 0 and 1
            var inputHorizontal = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT) - Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            var inputVertical = Input.GetAxisValue(InputConstants.K_MOVE_UP) - Input.GetAxisValue(InputConstants.K_MOVE_DOWN);

            // if the player is forced to walk, clamp their movement as if they were only moving the analog stick
            if (forceWalk && !isClimbing) {
                inputHorizontal = Mathf.Clamp(inputHorizontal, -0.25f, 0.25f);
                inputVertical = Mathf.Clamp(inputHorizontal, -0.25f, 0.25f);
            }

            if (IsAttacking)
            {
                // Freeze X Position
                // Maintain y velocity so Unity can do gravity stuff
                FootVelocity = new Vector2(0, CurrentVelocity.y);
            }
            else if (isClimbing)
            {
                // Gravity is disabled while climbing
                FootVelocity = new Vector2(inputHorizontal * moveSpeed, inputVertical * climbSpeed);
            }
            else {
                // Move as usual but again maintain the current y velocity so unity can do its thing
                FootVelocity = new Vector2(inputHorizontal * moveSpeed, CurrentVelocity.y);
            }

        }
        public void SetMovementLock(bool isLocked)
        {
            FootVelocity = Vector2.zero;
            isMovementLocked = isLocked;
        }

        public void SetForceWalk(bool isForced) {
            forceWalk = isForced;
        }


        // Called from unity event
        public void OnLandOnGround()
        {
            usedUppercut = false;
        }

        public void OnGetAttacked() {
            if (!isClimbing)
            {
                animator.DamageFront();
            }
        }

        public void DisableDeathScreen() => isDeathScreenEnabled = false;

        // These are disgusting hacks.  I just want to finish this damn thing.
        public bool IsInputMoveUp() {
            if (Input.GetActiveListener().GetType() == typeof(GamepadInputListener))
            {
                return Input.GetAxisValue(InputConstants.K_MOVE_UP) > 0.2f;
            }
            else {
                return Input.IsKeyHeld(InputConstants.K_MOVE_UP);
            }
        }

        public bool IsInputMoveDown()
        {
            if (Input.GetActiveListener().GetType() == typeof(GamepadInputListener))
            {
                return Input.GetAxisValue(InputConstants.K_MOVE_DOWN) > 0.2f;
            }
            else
            {
                return Input.IsKeyHeld(InputConstants.K_MOVE_DOWN);
            }
        }

        #region Animation Events - Triggered via Spine Animation
        // ****************************************************
        // ** These must be wired up via Unity's timeline ******
        // ****************************************************
        public void OnAttackBegin()
        {
            ActiveAttack = AttackTypes.Swing;
            IsAttacking = true;
        }

        public void OnUppercutBegin()
        {
            ActiveAttack = AttackTypes.Uppercut;
            IsAttacking = true;
        }
        

        public void OnGroundPoundBegin()
        {
            ActiveAttack = AttackTypes.GroundPound;
            IsAttacking = true;
        }

        public void OnAttackEnd()
        {
            ActiveAttack = AttackTypes.None;
            IsAttacking = false;
        }

        public void OnDamageEnable()
        {
            // Add force to the player 
            if (animator.SkeletonIsFlipped)
            {
                AddImpact(-dodgeSpeed, 0);
            }
            else
            {
                AddImpact(dodgeSpeed, 0);
            }

            meleeCollider.IsDamageEnabled = true;
        }

        public void OnDamageDisable()
        {
            meleeCollider.IsDamageEnabled = false;
        }

        public void OnGroundPoundLanded()
        {
            cameraEffector.LargeImpact();
        }

        public void OnEscapeFromBeingTied() 
        {
            isTiedUp = false;
        }

        public void OnFootstep() {
            footsteps.DoTippyTap();
        }

        public void OnBeginSwing0() => sound.Swing0();

        public void OnBeginSwing1() => sound.Swing1();

        public void OnBeginSwing2() => sound.Swing2();

        public void OnLadderClink1() => sound.LadderClink1();

        public void OnLadderClink2() => sound.LadderClink2();

        

        #endregion

    }
}

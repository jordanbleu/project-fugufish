using Assets.Source.Components.Actor;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Components.Cutscenes.Transformation;
using Assets.Source.Components.Timer;
using Assets.Source.Enums;
using System;
using UnityEngine;

namespace Assets.Source.Components.Behavior.Humanoid
{
    /// <summary>
    /// Final Boss works basically like the melee enemy but with extra steps
    /// </summary>
    public class BossEnemyBehavior : CommonPhysicsComponent
    {
        [SerializeField]
        [Tooltip("Drag player object here")]
        protected GameObject player;

        [SerializeField]
        private TransformationCutsceneDirector cutscene;

        // Tweakable values - probably won't need to touch
        private const float RANGE_FROM_PLAYER = 3f;
        private const float CLOSE_RANGE_FROM_PLAYER = 2f;
        private const float PRECISION = 0.25f;
        private const float THINK_TIME_MIN = 800;
        private const float THINK_TIME_MAX = 1200;
        private const int STAGGER_CHANCE = 100;
        private const float MOVE_SPEED = 1.5f;
        private const float THRUST_SPEED = 16f; 
        private bool isDamageEnabled = false;
        private bool isBrainEnabled = true; // todo:  This will be initially false in the real game

        // Required components
        private PlayerBrainComponent playerBrain;
        private MeleeComponent meleeCollider;
        private GameTimerComponent brainTimer;
        private FinalBossAnimatorComponent animator;
        private ActorComponent actor;
        private ActorComponent playerActor;

        // Prefabs

        // Whether or not the actor is currently doing an attack animation
        private bool isAttacking = false;
        private bool isStunned = false;

        public override void ComponentAwake()
        {
            base.ComponentAwake();
            animator = GetRequiredComponent<FinalBossAnimatorComponent>();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>();
            actor = GetRequiredComponent<ActorComponent>();
            playerActor = GetRequiredComponent<ActorComponent>(player);

        }

        public override void ComponentStart()
        {
            InitBrainTimer();
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            UpdateAnimator();
            MoveTowardsPlayer();
            UpdateMeleeCollider();
            base.ComponentUpdate();
        }

        public void AttackedByPlayer() {
            if (!isStunned)
            {
                // Decide if the enemy should stagger 
                var staggerChance = UnityEngine.Random.Range(0, 100);

                // 30% chance of staggering
                if (staggerChance <= 30)
                {
                    animator.Damage();
                }
            }
            else {
                // extra damage
                actor.DepleteHealth(50);
                animator.StunSquish();
                isStunned = false;
            }
        }

        private void UpdateAnimator()
        {
            animator.IsStunned = isStunned;
            animator.HorizontalMoveSpeed = FootVelocity.x;
        }

        // Moves the actor within range of the player
        private void MoveTowardsPlayer()
        {
            // if player is outside actor's range of motion, or theyre in the air
            if (!PlayerIsInRange() && !isStunned)
            {
                // if player is to my right, seek out the player's left side
                if (player.transform.position.x > transform.position.x)
                {
                    FootVelocity = MoveTowards(new Vector2(player.transform.position.x - RANGE_FROM_PLAYER, transform.position.y), PRECISION);
                }
                else
                {
                    FootVelocity = MoveTowards(new Vector2(player.transform.position.x - RANGE_FROM_PLAYER, transform.position.y), PRECISION);
                }
            }
            else
            {
                // else, simply face the player
                animator.FaceTowardsPosition(player.transform.position);
                FootVelocity = new Vector2(0, CurrentVelocity.y);

            }
        }

        private Vector2 MoveTowards(Vector2 vector2, float precision)
        {
            if (!isAttacking)
            {
                // No need for intelligent movement.  Final boss battle has no obstacles.
                if (!transform.position.x.IsWithin(precision, vector2.x))
                {
                    if (transform.position.x > vector2.x)
                    {
                        return new Vector2(-MOVE_SPEED, CurrentVelocity.y);
                    }
                    else
                    {
                        return new Vector2(MOVE_SPEED, CurrentVelocity.y);
                    }
                }
            }
            return new Vector2(0, CurrentVelocity.y);
        }

        // whether or not the player is within the range of this actor
        private bool PlayerIsInRange() => (player.transform.position.x >= (transform.position.x - RANGE_FROM_PLAYER) && player.transform.position.x < (transform.position.x + RANGE_FROM_PLAYER));
        private bool PlayerIsInCloseRange() => (player.transform.position.x >= (transform.position.x - CLOSE_RANGE_FROM_PLAYER) && player.transform.position.x < (transform.position.x + CLOSE_RANGE_FROM_PLAYER));


        // Simply flips the melee collider component in the direction the actor is facing
        private void UpdateMeleeCollider()
        {
            meleeCollider.IsFlipped = animator.SkeletonIsFlipped;
        }

        // Creates a GameTimerComponent that will dictate how quickly the AI acts
        private void InitBrainTimer()
        {
            var obj = new GameObject();

            var timer = obj.AddComponent<GameTimerComponent>();
            // instantiate as the boss' child and destroy the prefab
            var inst = Instantiate(obj, transform);
            inst.name = "Brain Timer";
            Destroy(obj);
            brainTimer = GetRequiredComponent<GameTimerComponent>(inst);
            brainTimer.Label = "Brain Timer";
            brainTimer.StartTime = UnityEngine.Random.Range(THINK_TIME_MIN, THINK_TIME_MAX);
            brainTimer.onTimerReachZero.AddListener(BrainUpdate);
            brainTimer.ResetTimer();
            brainTimer.StartTimer();
        }

        // This is the method called when the brain timer reaches zero
        private void BrainUpdate()
        {
            if (playerActor.Health > 0 && isBrainEnabled && !isAttacking && !isStunned && PlayerIsInRange()) {
                MeleeAttack();   
            }

            brainTimer.StartTime = UnityEngine.Random.Range(THINK_TIME_MIN, THINK_TIME_MAX);
            brainTimer.ResetTimer();
            brainTimer.StartTimer();
        }

        // Perform a melee attack
        private void MeleeAttack()
        {
            if (PlayerIsInCloseRange())
            {
                // Do a devastating close attack
                var whatToDo = UnityEngine.Random.Range(0, 5);

                if (whatToDo <= 2)
                {
                    // A basic downward attack
                    animator.BodySmash();
                }
                else
                {
                    // The more slow / devastating attack (less likely to happen)
                    animator.DownwardSmash();
                }
            }
            else
            {
                var whatToDo = UnityEngine.Random.Range(0, 10);
                if (whatToDo < 3)
                {
                    animator.OverheadSlice();
                }
                else if (whatToDo < 6)
                {
                    animator.SwordSlice();
                }
                else {
                    animator.ForwardSlash();
                }
            }
        }

        public void Stun()
        {
            isStunned = true;
        }

        public void SetEnabled(bool enabled) => isBrainEnabled = enabled;

        #region animation callbacks
        public void OnStandUp()
        {
            // todo: uncomment for final build
            //cutscene.IncrementStage();
        }

        public void OnAttackBegin()
        {
            isAttacking = true;
        }

        public void OnAttackEnd()
        {
            isAttacking = false;
        }

        public void OnDamageBegin()
        {
            isDamageEnabled = true;
            meleeCollider.IsDamageEnabled = true;
        }

        public void OnDamageEnd()
        {
            isDamageEnabled = false;
            meleeCollider.IsDamageEnabled = false;
        }

       
        public void OnThrustForward() {
            // Add force to the actor 
            if (animator.SkeletonIsFlipped)
            {
                AddImpact(-THRUST_SPEED, 0);
            }
            else
            {
                AddImpact(THRUST_SPEED, 0);
            }

        }

        public void OnStunEnd() {
            isStunned = false;
        }

        #endregion

        public override void DrawAdditionalGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - CLOSE_RANGE_FROM_PLAYER, transform.position.y-0.5f));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + CLOSE_RANGE_FROM_PLAYER, transform.position.y-0.5f));

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - RANGE_FROM_PLAYER, transform.position.y));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + RANGE_FROM_PLAYER, transform.position.y));
            
            base.DrawAdditionalGizmosSelected();
        }

    }

    
}

using Assets.Source.Components.Actor;
using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Timer;
using Assets.Source.Enums;
using System;
using UnityEngine;

namespace Assets.Source.Components.Behavior.Humanoid
{
    public class MeleeEnemyBehavior : HumanoidBehaviorBase
    {
        [SerializeField]
        [Header("Follow player Properties")]
        [Tooltip("How up close and personal the actor gets to the player")]
        private float rangeFromPlayer = 2f;

        [SerializeField]
        [Tooltip("How closely the actor needs to be to the player's position +/- range")]
        private float precision = 0.25f;

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMin;

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMax;

        private PlayerBrainComponent playerBrain;
        private IntervalTimerComponent brainTimer;
        private MeleeComponent meleeCollider;


        private bool shouldDodge = false;

        
        private bool isAttacking = false;

        public override void ComponentStart()
        {
            var brain = GetRequiredComponent<HumanoidNPCBrainComponent>();
            brain.attackByPlayer.AddListener(OnAttackedByPlayer);
            brain.attackBegin.AddListener(OnAttackBegin);
            brain.attackEnd.AddListener(OnAttackEnd);
            brain.damageEnable.AddListener(OnDamageEnable);
            brain.damageDisable.AddListener(OnDamageDisable);


            var brainTimerObject = new GameObject("BrainTimer");
            brainTimer = brainTimerObject.AddComponent<IntervalTimerComponent>();
            brainTimer.SetInterval((int)UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax)); 
            brainTimer.SelfDestruct = false;
            brainTimer.Randomize = false;
            brainTimer.AutoReset = true;
            brainTimer.OnIntervalReached.AddListener(BrainUpdate);
            Instantiate(brainTimerObject, transform);
            base.ComponentStart();
        }



        // Called from the interval timer
        private void BrainUpdate()
        {

            if (!isAttacking && PlayerIsInRange()) {
                Attack();
            }

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            brainTimer.SetInterval((int)UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax));
        }


        public override void ComponentAwake()
        {
            base.ComponentAwake();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>();
        }

        private void OnAttackedByPlayer()
        {
            if (playerBrain.ActiveAttack == AttackTypes.Uppercut)
            {
                AddRigidBodyForce(0, 10f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                AddImpact(15f, 0);
            }
            else
            {
                AddImpact(-15f, 0);
            }
        }

        public override void ComponentUpdate()
        {
            MoveTowardsPlayer();
            UpdateMeleeCollider();
            base.ComponentUpdate();
        }

        private void UpdateMeleeCollider()
        {
            meleeCollider.IsFlipped = animator.SkeletonIsFlipped;

        }

        private void MoveTowardsPlayer()
        {
            // if player is outside actor's range of motion, or theyre in the air
            if (!PlayerIsInRange())
            {
                // if player is to my right, seek out the player's left side
                if (player.transform.position.x > transform.position.x)
                {
                    FootVelocity = MoveIntelligentlyTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }
                else
                {
                    FootVelocity = MoveIntelligentlyTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }
            }
            else
            {
                // else, simply face the player
                animator.FaceTowardsPosition(player.transform.position);
                FootVelocity = new Vector2(0, CurrentVelocity.y);
            }

        }

        private void Attack()
        {
            animator.Attack();      
        }


        private bool PlayerIsInRange() => (player.transform.position.x > (transform.position.x - rangeFromPlayer) && player.transform.position.x < (transform.position.x + rangeFromPlayer));


        #region Event Handlers
        private void OnAttackBegin()
        {
            isAttacking = true;
        }

        private void OnAttackEnd()
        {
            isAttacking = false;
        }

        private void OnDamageEnable()
        {
            // Add force to the actor 
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

        private void OnDamageDisable()
        {
            meleeCollider.IsDamageEnabled = false;
        }
        #endregion

    }
}

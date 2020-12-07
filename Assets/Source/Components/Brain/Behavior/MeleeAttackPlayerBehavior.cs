using Assets.Source.Components.Actor;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Components.Timer;
using System;
using UnityEngine;

namespace Assets.Source.Components.Brain.Behavior
{

    [RequireComponent(typeof(IntervalTimerComponent))]
    public class MeleeAttackPlayerBehavior : AIBehaviorBase
    {
        [SerializeField]
        [Tooltip("Max speed the actor can move towards its destination")]
        private float moveSpeed = 4f;

        [SerializeField]
        [Tooltip("How high the actor jumps.")]
        private float jumpHeight = 12f;

        [SerializeField]
        [Tooltip("How up close and personal the actor gets to the player")]
        private float rangeFromPlayer = 2f;

        private Vector2 destination;
        private float movement;
        private bool isAttacking;

        // Components
        private MeleeComponent meleeCollider;

        public override void ComponentAwake()
        {
            base.ComponentAwake();
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>(Brain.gameObject);
            Brain.DamageEnable.AddListener(OnDamageEnable);
            Brain.DamageDisable.AddListener(OnDamageDisable);
            Brain.AttackBegin.AddListener(OnAttackBegin);
            Brain.AttackEnd.AddListener(OnAttackEnd);
        }

        private void OnAttackEnd()
        {
            isAttacking = false;
        }

        private void OnAttackBegin()
        {
            isAttacking = true;
        }

        private void OnDamageDisable()
        {
            meleeCollider.IsDamageEnabled = false;
        }

        private void OnDamageEnable()
        {
            meleeCollider.IsDamageEnabled = true;
        }

        public override void ComponentUpdate()
        {
            meleeCollider.IsFlipped = Brain.Animator.SkeletonIsFlipped;
            base.ComponentUpdate();
        }

        public override Vector2 HorizontalMovment()
        {
            // if player is outside actor's range of motion
            if (!PlayerIsInRange())
            {
                // if player is to my right, seek out the player's left side
                if (Player.transform.position.x > transform.position.x)
                {
                    destination = new Vector2(Player.transform.position.x - rangeFromPlayer, transform.position.y);
                }
                else
                {
                    destination = new Vector2(Player.transform.position.x + rangeFromPlayer, transform.position.y);
                }
                return SmartMoveTowards(destination, 0.25f, moveSpeed, jumpHeight);
            }
            else {
                Brain.Animator.FaceDirection(Player.transform.position.x < transform.position.x);
                return new Vector2(0f, Brain.CurrentVelocity.y);
            }
        }

        private bool PlayerIsInRange() => (Player.transform.position.x > (transform.position.x - rangeFromPlayer) && Player.transform.position.x < (transform.position.x + rangeFromPlayer));

        // Time for action.  This is called from an interval timer on the same object.  Changing the time will 
        // Make the AI faster and harder 
        public void OnTimerIntervalReached() {
            if (PlayerIsInRange()) { 
                if (!isAttacking) {
                    Brain.Animator.Attack();
                } 
            }
        }

        public override void DrawAdditionalGizmos()
        {
            if (UnityUtils.Exists(Player)) {
                // Currently seeking position is drawn in orange
                var clr = UnityUtils.Color(250, 100, 0);
                Gizmos.color = clr;
                Gizmos.DrawLine(transform.position, destination);

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position, new Vector3(rangeFromPlayer, 1, 1));
            }
            base.DrawAdditionalGizmos();
        }

        
    }
}

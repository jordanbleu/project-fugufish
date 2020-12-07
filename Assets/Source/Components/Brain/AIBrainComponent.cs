using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Enums;
using Assets.Source.Math;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Brain
{
    /// <summary>
    /// The AI Brain is a State Machine handles activation of separate behaviors based on their current state of mind.  
    /// Doing it this way is a bit janky but it keeps states totally uncoupled and clean.
    /// </summary>
    public class AIBrainComponent : CommonPhysicsComponent
    {
        [Header("Movement Boundaries")]
        [SerializeField]
        private bool useMovementBoundaries = true;
        public bool UseMovementBoundaries { get => useMovementBoundaries; }

        [SerializeField]
        [Tooltip("Drag an object with a behavior state here.  This will be the behavior when the player is NOT within boundaries.")]
        private GameObject idleStateObject;

        [SerializeField]
        [Tooltip("Drag an object with a behavior state here.  This will be the behavior when the player within boundaries.")]
        private GameObject activeStateObject;

        [SerializeField]
        [Tooltip("Actor will not leave this area (this is the magenta square).")]
        private Square movementBounds;

        [SerializeField]
        [Tooltip("Drag the player object here.")]
        private GameObject player;

        [SerializeField]
        [Tooltip("Force that the actor is propelled by player attacks")]
        private float playerHitForce;

        // Unity events - behaviors will have to subscribe to these to respond 
        public UnityEvent AttackBegin { get; set; } = new UnityEvent();
        public UnityEvent AttackEnd { get; set; } = new UnityEvent();
        public UnityEvent DamageEnable { get; set; } = new UnityEvent();
        public UnityEvent DamageDisable { get; set; } = new UnityEvent();


        // Components
        public HumanoidSkeletonAnimatorComponent Animator { get; private set; }

        public override void ComponentAwake()
        {
            Animator = GetRequiredComponent<HumanoidSkeletonAnimatorComponent>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // use either idle or active state
            var behavior = UpdateBehavior();

            // Apply the requested horizontal movement from the behavior
            UpdateMovement(behavior.HorizontalMovment());

            UpdateAnimator();

            base.ComponentUpdate();
        }

        private void UpdateAnimator()
        {
            Animator.IsClimbing = false;
            Animator.IsGrounded = IsGrounded;
            Animator.HorizontalMoveSpeed = FootVelocity.x;
            Animator.VerticalMoveSpeed = FootVelocity.y;
        }

        private void UpdateMovement(Vector2 movement)
        {
            // todo: y is currently disabled
            FootVelocity = new Vector2(movement.x, CurrentVelocity.y);
        }

        private AIBehaviorBase UpdateBehavior()
        { 
            if (!useMovementBoundaries || movementBounds.SurroundsPoint(player.transform.position))
            {
                if (!activeStateObject.activeInHierarchy || idleStateObject.activeInHierarchy)
                {
                    activeStateObject.SetActive(true);
                    idleStateObject.SetActive(false);
                }
                return GetRequiredComponent<AIBehaviorBase>(activeStateObject);
            }
            else
            {
                if (!idleStateObject.activeInHierarchy || activeStateObject.activeInHierarchy)
                {
                    activeStateObject.SetActive(false);
                    idleStateObject.SetActive(true);
                }
                return GetRequiredComponent<AIBehaviorBase>(idleStateObject);
            }
        }

        // Called from unity events
        public void HitByPlayer(GameObject player) {

            var brain = GetRequiredComponent<PlayerBrainComponent>(player);

            if (brain.ActiveAttack == AttackTypes.Swing)
            {
                // Apply Directional Velocity
                if (player.transform.position.x <= transform.position.x)
                {
                    AddImpact(playerHitForce, 0);
                }
                else
                {
                    AddImpact(-playerHitForce, 0);
                }
            }
            else if (brain.ActiveAttack == AttackTypes.Uppercut)
            {
                AddRigidBodyForce(0, playerHitForce);
            }
            else if (brain.ActiveAttack == AttackTypes.GroundPound) 
            {
                AddRigidBodyForce(0, -playerHitForce);
            }

        }

        internal void Jump(float height)
        {
            AddRigidBodyForce(0, height);
        }

        internal void MeleeAttack() {
            Animator.Attack();
        }

        public override void DrawAdditionalGizmosSelected()
        {
            // Draw brain's Boundaries
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(movementBounds.Center, new Vector2(movementBounds.Width, movementBounds.Height));
            base.DrawAdditionalGizmosSelected();
        }

        public override void OnAttackBegin()
        {
            AttackBegin?.Invoke();
            base.OnAttackBegin();
        }

        public override void OnAttackEnd()
        {
            AttackEnd?.Invoke();
            base.OnAttackEnd();
        }

        public override void OnDamageEnable()
        {
            DamageEnable?.Invoke();
            base.OnDamageEnable();
        }

        public override void OnDamageDisable()
        {
            DamageDisable?.Invoke();
            base.OnDamageDisable();
        }

    }
}

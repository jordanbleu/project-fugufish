using Assets.Editor.Attributes;
using Assets.Source.Components.Platforming;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Brain.Base
{
    /// <summary>
    /// Handles common physics for platforming type games in an abstract fashion.  
    /// If an actor exists in the physical world, it probably should inherit from this class.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class CommonPhysicsComponent : ComponentBase
    {
        [SerializeField]
        [Header("Common Physics")]
        [Tooltip("This is the size of the bottom of the object.  Helps determine if the object is on the ground or not.")]
        private float feetRadius = 0.15f;

        [SerializeField]
        [Tooltip("Set this to apply gravity scale to the rigid body.  NOTE: Rigid Body's gravity scale will be ignored so don't use that.")]
        private float gravityScale = 5f;

        [SerializeField]
        [Tooltip("If true, the actor can be affected by ForceComponents.")]
        private bool isAffectedByForceComponent = true;

        [SerializeField]
        [Tooltip("Delegate to invoke once the actor touches the ground after falling")]
        private UnityEvent onLand;

        [SerializeField]
        [Tooltip("Layers that the actor considers the ground")]
        protected LayerMask groundLayers;

        [SerializeField]
        [Tooltip("The maximum impact velocity.")]
        private float impactVelocityLimit = 12f;


        /// <summary>
        /// This is the velocity the actor is walking or jumping
        /// </summary>
        [SerializeField]
        [Header("Velocities")]
        [ReadOnly]
        private Vector2 footVelocity;
        protected Vector2 FootVelocity { get => footVelocity; set => footVelocity = value; }

        /// <summary>
        /// Constant velocity affecting the actor from outside (wind, water, converyor belt, etc)
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private Vector2 environmentalVelocity;
        protected Vector2 EnvironmentalVelocity { get => environmentalVelocity; private set => environmentalVelocity = value; }

        /// <summary>
        /// This is the velocity the actor is moving due to an impact (bouncing, getting pushed, getting slapped, etc).
        /// The values self-stabilize smoothly back to zero.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private Vector2 impactVelocity;
        protected Vector2 ImpactVelocity { get => impactVelocity; private set => impactVelocity = value; }

        // Track all colliding object triggers
        protected List<GameObject> CollidingTriggers { get; private set; } = new List<GameObject>();

        // Needed for friction hack
        private PhysicsMaterial2D grippyMaterial;
        private PhysicsMaterial2D slippyMaterial;

        // Components
        protected Rigidbody2D rigidBody2d;
        protected Collider2D collider2d;

        private bool wasGrounded = false;

        public bool IsGrounded { get; private set; }
        public Vector2 CurrentVelocity { get => rigidBody2d.velocity; }

        /// <summary>
        /// Enable or disable gravity for this actor.
        /// </summary>
        public bool IsGravityEnabled { get; set; } = true;

        public override void ComponentAwake()
        {
            grippyMaterial = new PhysicsMaterial2D("GrippyMaterial") { friction = 999f };
            slippyMaterial = new PhysicsMaterial2D("SlippyMaterial") { friction = 0f };
            rigidBody2d = GetRequiredComponent<Rigidbody2D>();
            collider2d = GetRequiredComponent<Collider2D>();
            base.ComponentAwake();
        }

        public override void ComponentFixedUpdate()
        {
            // Combine the velocity of all external forces and apply them to EnvironmentalVelocity
            UpdateEnvironmentalVelocity();

            // Smoothly decelerate the ImpactVelocity
            StabilizeImpactVelocity();

            // Enable or disable gravity on the rigid body
            UpdateGravity();

            // Check if we're standing on the ground
            CheckIfGrounded();

            // Apply all velocities to the rigid body
            ApplyTotalVelocity();

            // Prevent player from sliding if they're not moving horizontally
            ApplyFrictionHack();

            base.ComponentUpdate();
        }

        /// <summary>
        /// Increments the current impact velocity by the specified amount
        /// </summary>
        public void AddImpact(float x, float y) {
            var xv = Mathf.Clamp(ImpactVelocity.x + x, -impactVelocityLimit, impactVelocityLimit);
            var yv = Mathf.Clamp(ImpactVelocity.y + y, -impactVelocityLimit, impactVelocityLimit);

            ImpactVelocity = new Vector2(xv, yv);
        }

        /// <summary>
        /// Adds force directly to the rigid body.  Used for forces that should be stabilized by Unity itself, such 
        /// as via Unity's gravity, etc
        /// </summary>
        public void AddRigidBodyForce(float x, float y) => rigidBody2d.AddForce(new Vector2(x, y), ForceMode2D.Impulse);

        private void UpdateEnvironmentalVelocity()
        {
            var xForce = 0f;
            var yForce = 0f;

            // Calculates the total environmental force from ForceComponents
            foreach (var trigger in CollidingTriggers)
            {
                if (trigger.TryGetComponent<ForceComponent>(out var forceComponent))
                {
                    if (isAffectedByForceComponent && (forceComponent.IsGroundOnly || IsGrounded))
                    {
                        xForce += forceComponent.Amount.x;
                        yForce += forceComponent.Amount.y;
                    }
                }
            }

            EnvironmentalVelocity = new Vector2(xForce, yForce);
        }

        private void StabilizeImpactVelocity()
        {
            // todo: weight
            ImpactVelocity = new Vector2(ImpactVelocity.x.Stabilize(1, 0), ImpactVelocity.y.Stabilize(1, 0));
        }


        private void ApplyTotalVelocity()
        {
            // Combine all the velocities
            var xVelocity = FootVelocity.x + EnvironmentalVelocity.x + ImpactVelocity.x;
            var yVelocity = FootVelocity.y + EnvironmentalVelocity.y + ImpactVelocity.y;

            // todo: Integrate 'weight' scale

            // Apply to the rigid body
            rigidBody2d.velocity = new Vector2(xVelocity, yVelocity);
        }

        private void UpdateGravity()
        {
            if (!IsGravityEnabled)
            {
                rigidBody2d.gravityScale = 0f;
            }
            else
            {
                rigidBody2d.gravityScale = gravityScale;
            }
        }

        private void ApplyFrictionHack()
        {
            var combinedXVelocity = FootVelocity.x + EnvironmentalVelocity.x + ImpactVelocity.x;

            if (combinedXVelocity == 0f)
            {
                collider2d.sharedMaterial = grippyMaterial;
            }
            else
            {
                collider2d.sharedMaterial = slippyMaterial;
            }
        }

        private void CheckIfGrounded()
        {
            IsGrounded = false;

            // Represents the bottom area of the collider, which we call its "feet"

            Collider2D[] colliders = Physics2D.OverlapCircleAll(GetFeetCenter(), feetRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                // If the game object we're colliding with:
                // * Is not ourself
                // * Is not a trigger
                // * Is included in our ground layers layermask
                if (colliders[i].gameObject != gameObject && (!colliders[i].isTrigger) && groundLayers.IncludesLayer(colliders[i].gameObject.layer))
                {
                    IsGrounded = true;

                    if (!wasGrounded)
                    {
                        onLand?.Invoke();
                    }
                }
            }

            wasGrounded = IsGrounded;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CollidingTriggers.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CollidingTriggers.Remove(collision.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            // draw the "feet" area
            if (collider2d != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(GetFeetCenter(), feetRadius);
            }
            DrawAdditionalGizmosSelected();
        }

        /// <summary>
        /// Used to layer more gizmos on top of the common physics gizmos
        /// </summary>
        public virtual void DrawAdditionalGizmosSelected() { }

        public Vector2 GetFeetCenter()
        {

            var bottomThird = collider2d.bounds.center.y - (collider2d.bounds.size.y / 2);
            return new Vector3(collider2d.bounds.center.x, bottomThird);
        }
    }
}

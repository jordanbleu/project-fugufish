using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Hazards
{
    /// <summary>
    /// Any object with a Hazard Component will damage the player on collision.
    /// Works with either collision triggers or solid colliders
    /// </summary>
    [Obsolete("I'm not sure if this should be used anymore")]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class HazardComponent : ComponentBase
    {


        [SerializeField]
        private UnityEvent onCollideWithPlayer;

        [SerializeField]
        [Tooltip("if true, force will be applied in the direction that the object is moving")]
        private bool forceDirectionAffectedByVelocity = true;
        public bool ForceDirectionAffectedByVelocity { get => forceDirectionAffectedByVelocity; }

        [SerializeField]
        [Tooltip("If true, the player's position on impact will also affect the direction of the force")]
        private bool forceDirectionAffectedByColliderPosition = true;
        public bool ForceDirectionAffectedByColliderPosition { get => forceDirectionAffectedByColliderPosition; }

        [SerializeField]
        [Tooltip("Force that gets applied to the player on contact")]
        private Vector2 force = new Vector2(0,0);
        public Vector2 Force { get => force; }

        /// <summary>
        /// Returns the current velocity of the rigid body 
        /// </summary>
        public Vector2 Velocity { get => rigidBody.velocity; }

        private Rigidbody2D rigidBody;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentAwake();
        }

        public void OnPlayerHit() {

            onCollideWithPlayer?.Invoke();
        }

        public void KillSelf() {
            Destroy(gameObject);
        }

    }
}

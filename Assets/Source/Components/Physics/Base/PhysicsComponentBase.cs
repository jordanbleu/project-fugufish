using UnityEditor.UIElements;
using UnityEngine;

namespace Assets.Source.Components.Physics.Base
{
    /// <summary>
    /// Represents a physical object in the world
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PhysicsComponentBase : ComponentBase
    {
        private Rigidbody2D rigidBody;
        protected Collider2D Collider { get; private set; }

        protected bool IsGrounded { get; private set; }

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            Collider = GetRequiredComponent<Collider2D>();
            base.ComponentAwake();
        }

        public override void ComponentFixedUpdate()
        {
            var xVelocity = 0f;
            var yVelocity = 0f;

            xVelocity += CalculateHorizontalMovement();

            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
            base.ComponentFixedUpdate();
        }

        public virtual float CalculateHorizontalMovement() => 0f;

        protected void AddForce(Vector2 force) 
        {
            rigidBody.AddForce(force);
        }



    }
}

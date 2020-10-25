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

        private float gravityScale;

        protected bool IsGrounded { get; private set; }
        protected bool IsGravityEnabled { get; set; }

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            Collider = GetRequiredComponent<Collider2D>();
            gravityScale = rigidBody.gravityScale;
            base.ComponentAwake();
        }

        public override void ComponentFixedUpdate()
        {
            var xVelocity = 0f;
            var yVelocity = 0f;

            xVelocity += CalculateHorizontalMovement();
            yVelocity += CalculateVerticalMovement();

            if (!IsGravityEnabled)
            {
                rigidBody.gravityScale = 0;
            }
            else
            {
                rigidBody.gravityScale = gravityScale;   
            }
            
            rigidBody.velocity = new Vector2(xVelocity, yVelocity);
            base.ComponentFixedUpdate();
        }

        public virtual float CalculateHorizontalMovement() => 0f;
        public virtual float CalculateVerticalMovement() => 0f;

        protected void AddForce(Vector2 force) 
        {
            rigidBody.AddForce(force);
        }

        protected Vector2 GetVelocity() => rigidBody.velocity;



    }
}

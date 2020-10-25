using Assets.Source.Extensions;
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

        /// <summary>
        /// External forces that affect the player, such as wind, water, conveyor belts, etc
        /// </summary>
        protected Vector2 ExternalVelocity { get; set; }

        protected bool IsGrounded { get; private set; }
        protected bool IsGravityEnabled { get; set; }

        public override void ComponentAwake()
        {
            ExternalVelocity = Vector2.zero;
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
            
            rigidBody.velocity = new Vector2(xVelocity, yVelocity) + ExternalVelocity;

            base.ComponentFixedUpdate();
        }



        /// <summary>
        /// The current driving horizontal movement force (walking, running, driving, etc)
        /// </summary>
        /// <returns></returns>
        public virtual float CalculateHorizontalMovement() => 0f;
        /// <summary>
        /// The current driving vertical movement force (jumping, etc)
        /// </summary>
        /// <returns></returns>
        public virtual float CalculateVerticalMovement() => 0f;


        protected void AddForce(Vector2 force) 
        {
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }

        protected Vector2 GetVelocity() => rigidBody.velocity;



    }
}

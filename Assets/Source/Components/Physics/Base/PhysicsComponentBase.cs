using Assets.Source.Components.Player;
using UnityEngine;

namespace Assets.Source.Components.Physics.Base
{
    /// <summary>
    /// Represents a physical object in the world
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PhysicsComponentBase : ComponentBase
    {

        [SerializeField]
        [Tooltip("Adjusts the size of the bottom of the object.  " +
            "This is used to calculate whether the object is grounded or not." +
            "With the object selected, use the cyan circle to determine.")]
        private float feetRadius = 2f;


        private Rigidbody2D rigidBody;
        protected Collider2D Collider { get; private set; }

        private float gravityScale;

        /// <summary>
        /// External forces that affect the player, such as wind, water, conveyor belts, etc
        /// </summary>
        protected Vector2 ExternalVelocity { get; set; }

        [SerializeField]
        private bool isGrounded = false;
        public bool IsGrounded => isGrounded;
        
        
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
            
            CheckIfGrounded();
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

        // Checks if the player is on the ground currently
        private void CheckIfGrounded()
        {
            isGrounded = false; 

            // Represents the bottom area of the collider, which we assume to be the "feet
            var bottomThird = Collider.bounds.center.y - (Collider.bounds.size.y / 2);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(Collider.bounds.center.x, bottomThird), feetRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                // Check that the collider isn't the player or a child object of the player
                if (colliders[i].gameObject != gameObject && colliders[i].GetComponentInParent<PlayerPhysicsComponent>() == null)
                {
                    if (!colliders[i].isTrigger) { 
                        isGrounded = true;
                    }

                    // todo:  I think we want to implement this soon
                    //if (!wasGrounded)
                    //    OnLandEvent.Invoke();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // draw the bottom bit
            if (Collider != null) {
                var bottomThird = Collider.bounds.center.y - (Collider.bounds.size.y / 2);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(new Vector3(Collider.bounds.center.x, bottomThird,0), feetRadius);
            }
        }

        

    }
}

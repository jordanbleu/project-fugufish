using Assets.Editor.Attributes;
using Assets.Source.Components.Player;
using UnityEngine;
using UnityEngine.Events;

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
        private float feetRadius = 0.25f;

        private Rigidbody2D rigidBody;
        protected Collider2D Collider { get; private set; }
        
        private PhysicsMaterial2D grippyMaterial;
        private PhysicsMaterial2D slippyMaterial;

        private float gravityScale;

        /// <summary>
        /// External forces that affect the player, such as wind, water, conveyor belts, etc
        /// </summary>
        protected Vector2 ExternalVelocity { get; set; }

        // Temporary jus to show inspector
        [ReadOnly]
        public Vector2 CurrentExtVelcoityhh;
        
        [SerializeField]
        [ReadOnly]
        private bool isGrounded = false;
        public bool IsGrounded => isGrounded;

        // tracks if the player was grounded in the last frame
        private bool wasGrounded = false;

        [SerializeField]
        [Tooltip("Event that fires when the actor hits the ground for the first time")]
        private UnityEvent onLand;

        protected bool IsGravityEnabled { get; set; } = true;

        public override void ComponentAwake()
        {
            grippyMaterial = new PhysicsMaterial2D("GrippyMaterial") { friction = 999f };
            slippyMaterial = new PhysicsMaterial2D("SlippyMaterial") { friction = 0f };

            ExternalVelocity = Vector2.zero;
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            Collider = GetRequiredComponent<Collider2D>();
            gravityScale = rigidBody.gravityScale;

            base.ComponentAwake();
        }

        public override void ComponentFixedUpdate()
        {
            CurrentExtVelcoityhh = ExternalVelocity;// Temp delete pls

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
            AdjustFriction(xVelocity, ExternalVelocity);
            rigidBody.velocity = new Vector2(xVelocity, yVelocity) + ExternalVelocity;
            wasGrounded = isGrounded;
            base.ComponentFixedUpdate();
        }

        // This is a slightly hacky solution that dynamically swaps physics materials to 
        // stop the player from sliding down slopes
        private void AdjustFriction(float xVelocity, Vector2 externalVelocity)
        {
            var combinedXVelocity = xVelocity + externalVelocity.x;
            
            if (combinedXVelocity != 0f)
            {
                Collider.sharedMaterial = slippyMaterial;
            }
            else {
                Collider.sharedMaterial = grippyMaterial;
            }
        }

        /// <summary>
        /// The current driving horizontal movement force (walking, running, driving, etc)
        /// </summary>
        /// <returns></returns>
        public abstract float CalculateHorizontalMovement();
        /// <summary>
        /// The current driving vertical movement force (jumping, etc)
        /// </summary>
        /// <returns></returns>
        public abstract float CalculateVerticalMovement();


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

                        if (!wasGrounded) {
                            onLand?.Invoke();                    
                        }

                    }

                    
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

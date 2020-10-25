using System.Collections.Generic;
using UnityEngine;
using Assets.Source.Components;

namespace Assets.Source.Components.PlatformerPhysics
{

    public class PhysicsObject : ComponentBase
    {

        public float minGroundNormalY = .65f;
        public float gravityModifier = 1f;

        protected Vector2 targetVelocity;

        protected bool climbing;

        protected bool grounded;
        protected Vector2 groundNormal;
        protected Rigidbody2D rb2d;
        protected Vector2 velocity;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


        // Minimum distance for collision detection.  Ensures we don't check collisions when not moving
        protected const float minMoveDistance = 0.001f;
        // Padding around colliders to ensure player doesn't get stuck inside things
        protected const float shellRadius = 0.01f;

        void OnEnable()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public override void ComponentStart()
        {
            contactFilter.useTriggers = false;
            // Pull a layer mask from the Unity project settings
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
            base.ComponentUpdate();
        }


        protected virtual void ComputeVelocity() { }


        public override void ComponentFixedUpdate()
        {
            // Gravity

            if (!climbing) { 
                velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            }
            
            velocity.x = targetVelocity.x;

            grounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime;

            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            Vector2 move = moveAlongGround * deltaPosition.x;

            // Horizontal Movement
            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            // Vertical movement     
            if (climbing)
            {
            }
            else
            { 
                Movement(move, true);
            }
            
            base.ComponentFixedUpdate();
        }

        private void Movement(Vector2 move, bool yMovement)
        {
            // The overall distance we are moving
            float distance = move.magnitude;

            // Only perform collision checks if we are moving
            if (distance > minMoveDistance)
            {
                // Checks if we are going to be colliding with anything in the next frame
                int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

                hitBufferList.Clear();

                // Add all non-null results to a list
                for (int i = 0; i < count; i++)
                {
                    hitBufferList.Add(hitBuffer[i]);
                }

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    Vector2 currentNormal = hitBufferList[i].normal;

                    // Compares the slope (compared to velocity direction) 
                    // of the platform to determine if it is flat enough to stand on
                    if (currentNormal.y > minGroundNormalY)
                    {
                        grounded = true;
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    // Cancels out movement if we bump a ceiling
                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        velocity = velocity - projection * currentNormal;
                    }

                    // Prevents us from getting stuck inside other colliders
                    float modifiedDistance = hitBufferList[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }


            }

            rb2d.position = rb2d.position + move.normalized * distance;
        }

    }
}

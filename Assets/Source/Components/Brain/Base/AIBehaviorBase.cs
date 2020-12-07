using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Components.Brain.Base
{
    public abstract class AIBehaviorBase : ComponentBase
    {
        // Controls how far down the player looks under their feet
        private const float LOOK_BELOW_HEIGHT = 0.5f;

        [SerializeField]
        [Tooltip("Drag the player here")]
        private GameObject player;
        protected GameObject Player { get => player; }

        [SerializeField]
        [Tooltip("How far ahead the actor can see closer obstacles")]
        private float lookaheadDistance1 = 0.25f;
        protected float LookaheadDistance1 { get => lookaheadDistance1; }

        [SerializeField]
        [Tooltip("How far ahead the actor can see far obstacles")]
        private float lookaheadDistance2 = 3;
        protected float LookaheadDistance2 { get => lookaheadDistance2; }

        [SerializeField]
        [Tooltip("What layers contain objects that the actor can walk on")]
        private LayerMask walkableLayers;

        protected AIBrainComponent Brain { get; private set; }

        public override void ComponentAwake()
        {
            Brain = GetRequiredComponentInParent<AIBrainComponent>();
            base.ComponentAwake();
        }

        /// <summary>
        /// Used to return horizontal movement to be applied to physics component
        /// </summary>
        /// <returns></returns>
        public abstract Vector2 HorizontalMovment();

        /// <summary>
        /// Returns the velocity needed to move towards the destination.  The player will jump over any obstacles 
        /// if it can, or wait for moving platforms.  
        /// </summary>
        /// <param name="destination">The destination to move towards</param>
        /// <param name="precision">How close the actor has to be</param>
        /// <param name="moveSpeed">The speed to move</param>
        /// <param name="jumpHeight">How high u jump</param>
        /// <returns></returns>
        public Vector2 SmartMoveTowards(Vector2 destination, float precision, float moveSpeed, float jumpHeight) {

            // We are at our destination (or close enough) so return.
            if (transform.position.x.IsWithin(precision, destination.x))
            {
                return new Vector2(0, Brain.CurrentVelocity.y);
            }

            // Face left if the destination is to our left
            Brain.Animator.FaceDirection(destination.x < transform.position.x);

            // Figure out what direction we're in based on our animator
            var direction = (Brain.Animator.SkeletonIsFlipped) ? -1 : 1;

            // Do a ray cast to see what is in front of us
            var footCenter = Brain.GetFeetCenter();

            // Check if there is floor in front of us
            var closeHit = Physics2D.Raycast(new Vector2(footCenter.x, footCenter.y- LOOK_BELOW_HEIGHT), new Vector2(direction, 0), lookaheadDistance1, walkableLayers, -999, 999);

            // if there's a solid in front of us or we're falling
            if (closeHit.transform != null || !Brain.IsGrounded)
            {
                if (transform.position.x < destination.x)
                {
                    return new Vector2(moveSpeed, Brain.CurrentVelocity.y);
                }
                else if (transform.position.x > destination.x)
                {
                    return new Vector2(-moveSpeed, Brain.CurrentVelocity.y);
                }
            }
            else if (Brain.IsGrounded)
            {
                // Perform a secondary raycast to see if we can jump to something
                var farHit = Physics2D.Raycast(new Vector2(footCenter.x, footCenter.y - LOOK_BELOW_HEIGHT), new Vector2(direction, 0), lookaheadDistance2, walkableLayers, -999, 999);

                if (farHit.transform != null)
                {
                    // Jump up in the air.  Next frame we will move towards the player
                    Brain.Jump(jumpHeight);
                }
            }

            // There is nothing we can do, so wait here sadly.
            return new Vector2(0, Brain.CurrentVelocity.y);
        }

        public virtual void DrawAdditionalGizmos() { }

        private void OnDrawGizmosSelected()
        {
            if (UnityUtils.Exists(Brain)) {
                var feetCenter = Brain.GetFeetCenter();

                // If we are facing left
                if (Brain.Animator.SkeletonIsFlipped) {
                    Gizmos.color = UnityUtils.Color(200, 50, 200);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x - lookaheadDistance1, feetCenter.y- LOOK_BELOW_HEIGHT));
                    Gizmos.color = UnityUtils.Color(200, 25, 100);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x - lookaheadDistance2, feetCenter.y- LOOK_BELOW_HEIGHT));
                } else {
                    Gizmos.color = UnityUtils.Color(200, 50, 200);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x + lookaheadDistance1, feetCenter.y- LOOK_BELOW_HEIGHT));
                    Gizmos.color = UnityUtils.Color(200, 25, 100);
                    Gizmos.DrawLine(feetCenter, new Vector2(feetCenter.x + lookaheadDistance2, feetCenter.y- LOOK_BELOW_HEIGHT));
                }
                DrawAdditionalGizmos();
            }            
        }



    }
}

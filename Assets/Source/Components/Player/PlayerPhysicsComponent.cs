using Assets.Source.Components.Physics;
using Assets.Source.Components.Physics.Base;
using Assets.Source.Extensions;
using Assets.Source.Input.Constants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerPhysicsComponent : PhysicsComponentBase
    {

        [SerializeField]
        private float moveSpeed = 6f;

        [SerializeField]
        [Tooltip("How high player can jump.  You might need to tweak the object's mass as well")]
        private float jumpHeight = 16f;

        [SerializeField]
        private float dodgeMaxSpeed = 24f;

        [SerializeField]
        private float dodgeDeceleration = 1f;

        [SerializeField]
        private float climbingSpeed = 3f;

        [SerializeField]
        private List<GameObject> collidingTriggers = new List<GameObject>();

        private bool isClimbing = false;

        // Current speed at which the player is dodging 
        private float currentDodgeVelocity = 0f;

        public override void ComponentUpdate()
        {
            isClimbing = collidingTriggers.Any(tr => tr.GetComponent<LadderComponent>() != null);

            if (!isClimbing) 
            {
                if (Input.IsKeyPressed(InputConstants.K_JUMP))
                {
                    AddForce(new Vector2(0, jumpHeight));
                }
            }

            
            if (Input.IsKeyPressed(InputConstants.K_DODGE_LEFT)) 
            {
                currentDodgeVelocity -= dodgeMaxSpeed;
            }

            if (Input.IsKeyPressed(InputConstants.K_DODGE_RIGHT))
            {
                currentDodgeVelocity += dodgeMaxSpeed;
            }

            ExternalVelocity = new Vector2(currentDodgeVelocity, 0);

            StabilizeDodgeSpeed();
            base.ComponentUpdate();
        }

        private void StabilizeDodgeSpeed()
        {
            currentDodgeVelocity = currentDodgeVelocity.Stabilize(dodgeDeceleration, 0);
        }

        public override float CalculateHorizontalMovement()
        {
            var moveRight = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            var moveLeft = Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            return (moveRight - moveLeft) * moveSpeed;
        }

        public override float CalculateVerticalMovement()
        {
            if (isClimbing)
            {
                IsGravityEnabled = false;

                // if climbing we have to disable normal physics and do our own thing
                var moveUp = Input.GetAxisValue(InputConstants.K_MOVE_UP);
                var moveDown = Input.GetAxisValue(InputConstants.K_MOVE_DOWN);
                return (moveUp - moveDown) * climbingSpeed;
            }
            else 
            {
                IsGravityEnabled = true;
            }
            return GetVelocity().y;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collidingTriggers.Add(collision.gameObject);

            if (collision.gameObject.GetComponent<LadderComponent>() != null) { 
                // if we hit a ladder, cancel out our y velocity

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collidingTriggers.Remove(collision.gameObject);
        }


    }
}

using Assets.Source.Input.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.PlatformerPhysics
{

    public class PlayerPlatformerController : PhysicsObject
    {

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        [SerializeField]
        private List<GameObject> collidingTriggers = new List<GameObject>();

        // Use this for initialization
        public override void ComponentAwake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            base.ComponentAwake();
        }

        protected override void ComputeVelocity()
        {


            // Check if any colliding trigger is a ladder
            var climbing  = collidingTriggers.FirstOrDefault(go => go.GetComponent<LadderComponent>() != null) != null;

            if (!climbing)
            {
                if (Input.IsKeyPressed(InputConstants.K_JUMP) && grounded)
                {
                    velocity.y = jumpTakeOffSpeed;
                }
                else if (Input.IsKeyReleased(InputConstants.K_JUMP))
                {
                    if (velocity.y > 0)
                    {
                        velocity.y = velocity.y * 0.5f;
                    }
                }
            }


            //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            //if (flipSprite)
            //{
            //    spriteRenderer.flipX = !spriteRenderer.flipX;
            //}

            //animator.SetBool("grounded", grounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            Vector2 move = Vector2.zero;

            var moveRight = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            var moveLeft = Input.GetAxisValue(InputConstants.K_MOVE_LEFT);
            var moveUp = Input.GetAxisValue(InputConstants.K_MOVE_UP);

            move.x = (moveRight - moveLeft);
            targetVelocity = move * maxSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collidingTriggers.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collidingTriggers.Remove(collision.gameObject);
        }


    }
}

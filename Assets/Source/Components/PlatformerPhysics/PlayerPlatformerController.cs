using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.PlatformerPhysics
{

    public class PlayerPlatformerController : PhysicsObject
    {

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        public float rAxis = 0f;
        public float laxis = 0f;
        public bool isOnGrnd = false;

        // Use this for initialization
        public override void ComponentAwake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            base.ComponentAwake();
        }

        protected override void ComputeVelocity()
        {
            Vector2 move = Vector2.zero;

            // temp, delet
            isOnGrnd = grounded;
            rAxis = Input.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            laxis = Input.GetAxisValue(InputConstants.K_MOVE_LEFT);

            move.x = (rAxis - laxis);

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

            //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            //if (flipSprite)
            //{
            //    spriteRenderer.flipX = !spriteRenderer.flipX;
            //}

            //animator.SetBool("grounded", grounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }
    }
}

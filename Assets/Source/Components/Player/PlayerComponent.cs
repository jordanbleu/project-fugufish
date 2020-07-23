using Assets.Source.Components.Timer;
using Assets.Source.Input.Constants;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerComponent : ComponentBase
    {
        private Rigidbody2D rigidBody;
        private Animator animator;
        private MeshRenderer meshRenderer;
        private SkeletonMecanim skeletonMecanim;

        private IntervalTimerComponent dodgeTimer;

        [SerializeField]
        private float moveSpeed = 0.75f;

        [SerializeField]
        private float sprintSpeed = 1.5f;

        [SerializeField]
        private float dodgeSpeed = 2f;

        [SerializeField]
        private float _swingMovementSpeed = 2f;
        public float SwingMovementSpeed { get => _swingMovementSpeed; }

        private Directions currentDodgeDirection = Directions.None;

        public Directions DirectionFacing { get; private set; } = Directions.Up;

        public bool OverrideMovement { get; set; } = false;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            animator = GetRequiredComponent<Animator>();
            meshRenderer = GetRequiredComponent<MeshRenderer>();
            skeletonMecanim = GetRequiredComponent<SkeletonMecanim>();

            dodgeTimer = GetRequiredComponent<IntervalTimerComponent>(GetRequiredChild("DodgeInterval"));
            dodgeTimer.OnIntervalReached.AddListener(EndDodge);

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (!OverrideMovement)
            {
                UpdateDodges();
                UpdateDirection();

                if (currentDodgeDirection == Directions.None)
                {
                    UpdateMovement();
                }
            }
            
            if (currentDodgeDirection == Directions.None)
            {
                UpdateSwings();
            }
            base.ComponentUpdate();
        }

        private void UpdateDirection()
        {
            animator.SetInteger("direction_facing", (int)DirectionFacing);

            if (InputManager.IsKeyPressed(InputConstants.K_MOVE_UP))
            {
                DirectionFacing = Directions.Up;
            }
            else if (InputManager.IsKeyPressed(InputConstants.K_MOVE_DOWN))
            {
                DirectionFacing = Directions.Down;
            }

            if (DirectionFacing == Directions.Up)
            {
                skeletonMecanim.Skeleton.ScaleX = Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);
            }
            else
            {
                skeletonMecanim.Skeleton.ScaleX = -Mathf.Abs(skeletonMecanim.Skeleton.ScaleX);
            }
        }

        private void UpdateDodges()
        {
            meshRenderer.enabled = currentDodgeDirection == Directions.None;

            if (currentDodgeDirection == Directions.None)
            {
                if (InputManager.IsKeyPressed(InputConstants.K_DODGE_LEFT))
                {
                    currentDodgeDirection = Directions.Left;
                    dodgeTimer.Reset();
                }
                else if (InputManager.IsKeyPressed(InputConstants.K_DODGE_RIGHT))
                {
                    currentDodgeDirection = Directions.Right;
                    dodgeTimer.Reset();
                }
            }
            else 
            {
                if (currentDodgeDirection == Directions.Left)
                {
                    rigidBody.velocity = new Vector2(-dodgeSpeed, 0);
                }
                else {
                    rigidBody.velocity = new Vector2(dodgeSpeed, 0);
                }
            }
        }

        // Called from the interval timer when a dodge completes
        private void EndDodge() {
            currentDodgeDirection = Directions.None;
        }

        private void UpdateSwings()
        {
            if (InputManager.IsKeyPressed(InputConstants.K_SWING_SWORD)) {
                animator.SetTrigger("swing_sword");
            }
        }

        private void UpdateMovement()
        {
            var isSprinting = InputManager.IsKeyHeld(InputConstants.K_SPRINT);
            var horizontalMove = -(InputManager.GetAxisValue(InputConstants.K_MOVE_LEFT)) + InputManager.GetAxisValue(InputConstants.K_MOVE_RIGHT);
            var verticalMove = -(InputManager.GetAxisValue(InputConstants.K_MOVE_DOWN)) + InputManager.GetAxisValue(InputConstants.K_MOVE_UP);

            var adjustedMoveSpeed = isSprinting ? sprintSpeed : moveSpeed;

            rigidBody.velocity = new Vector2(horizontalMove * adjustedMoveSpeed, verticalMove * adjustedMoveSpeed);

            UpdateAnimator(horizontalMove, verticalMove, isSprinting);
        }

        private void UpdateAnimator(float horizontal, float vertical, bool isSprinting) 
        {
            var hTotal = Mathf.Abs(horizontal);
            var vTotal = Mathf.Abs(vertical);

            if (isSprinting && (hTotal > 0 || vTotal > 0))
            {
                animator.SetFloat("movement_type", (float)MovementType.Sprint);
            }
            else if (vTotal >= 0.75 || hTotal >= 0.75)
            {
                animator.SetFloat("movement_type", (float)MovementType.Run);
            }
            else if (vTotal > 0 || hTotal > 0)
            {
                animator.SetFloat("movement_type", (float)MovementType.Walk);
            }
            else
            {
                animator.SetFloat("movement_type", (float)MovementType.None);
            }
        }
    }
}

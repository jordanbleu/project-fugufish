using Assets.Source.Components.Level;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using System;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    /// <summary>
    /// Handles player controls and behavior
    /// </summary>
    public class PlayerComponent : ComponentBase
    {

        [SerializeField]
        private float movementSpeed =2f;

        private Rigidbody2D rigidBody;

        private Vector2 movementVelocity;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            CalculateMovement();

            ApplyVelocity();
            base.ComponentUpdate();
        }

        // Combine all velocities / physics to calculate actual velocity
        private void ApplyVelocity()
        {
            rigidBody.AddForce(movementVelocity);
        }

        // Add player's movement velocity from user controls
        private void CalculateMovement()
        {
            if (Input.IsKeyHeld(InputConstants.K_MOVE_LEFT))
            {
                movementVelocity = new Vector2(-movementSpeed, 0); ;
            }
            else if (Input.IsKeyHeld(InputConstants.K_MOVE_RIGHT))
            {
                movementVelocity = new Vector2(movementSpeed, 0);
            }
            else
            {
                movementVelocity = Vector2.zero;
            }
        }
    }
}

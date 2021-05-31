using Assets.Source.Components.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Platforming
{
 
    /// <summary>
    /// Moving Platforms simply loop among multiple platforming instructions
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class MovingPlatformComponent : ComponentBase
    {
        [SerializeField]
        private MoveMode MovementBehavior;

        [SerializeField]
        private List<PlatformInstruction> instructions;

        private float positionTolerance = 0.1f;
        
        private int index;
        private int indexDirection = 1;

        private Rigidbody2D rigidBody;
        private IntervalTimerComponent timer;
        
        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            
            timer = gameObject.AddComponent<IntervalTimerComponent>();
            timer.IsActive = false;
            timer.AutoReset = false;
            timer.OnIntervalReached = new UnityEngine.Events.UnityEvent();
            timer.OnIntervalReached.AddListener(GetNextInstruction);
            timer.AutoReset = false;

            index = 0;
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // Make sure index isn't out of bounds (can happen if manipulating values in inspector)
            index = Mathf.Min(index, instructions.Count());

            var currentInstruction = instructions[index];

            if (!timer.IsActive)
            {
                if (IsNearDestination(currentInstruction))
                {
                    timer.SetInterval(currentInstruction.WaitTime);
                    timer.IsActive = true;   
                }
            }

            UpdateVelocity(currentInstruction);
            
            base.ComponentUpdate();
        }

        private void GetNextInstruction()
        {
            if (MovementBehavior != MoveMode.Manual)
            {
                if (MovementBehavior == MoveMode.Random)
                {
                    index = UnityEngine.Random.Range(0, (instructions.Count()));
                }
                index += indexDirection;

                if (index > (instructions.Count() - 1) || index < 0)
                {
                    if (MovementBehavior == MoveMode.Cycle)
                    {
                        index = 0;
                    }
                    else
                    {
                        indexDirection = -indexDirection;
                        index += indexDirection;
                    }
                }
            }
        }

        public void CycleNext() {
            
            index += 1;

            if (index > (instructions.Count() - 1))
            {
                index = 0;
            }

        }

        public void CyclePrev() {
            index += 1;

            if (index < 0)
            {
                index = instructions.Count() - 1;
            }
        }

        private bool IsNearDestination(PlatformInstruction currentInstruction) =>        
            (transform.position.x.IsWithin(positionTolerance, currentInstruction.Position.x)) &&
            (transform.position.y.IsWithin(positionTolerance, currentInstruction.Position.y));
        

        private void UpdateVelocity(PlatformInstruction currentInstruction)
        {
            float xv = 0f;
            float yv = 0f;
            float xp = rigidBody.position.x;
            float yp = rigidBody.position.y;


            // Move x towards instruction's x position
            if (!transform.position.x.IsWithin(positionTolerance, currentInstruction.Position.x))
            {               
                if (transform.position.x < currentInstruction.Position.x)
                {
                    xv = currentInstruction.MovementSpeed;
                }
                else if (transform.position.x > currentInstruction.Position.x) {
                    xv = -currentInstruction.MovementSpeed;
                }
            }
            else
            {
                xp = currentInstruction.Position.x;
            }

            // Move y towards y position
            if (!transform.position.y.IsWithin(positionTolerance, currentInstruction.Position.y))
            {
                if (transform.position.y < currentInstruction.Position.y)
                {
                    yv = currentInstruction.MovementSpeed;
                }
                else if (transform.position.y > currentInstruction.Position.y)
                {
                    yv = -currentInstruction.MovementSpeed;
                }
            }
            else {
                yp = currentInstruction.Position.y;
            }

            rigidBody.position = new Vector2(xp, yp);
            rigidBody.velocity = new Vector2(xv, yv);
        }

        [Serializable]
        public struct PlatformInstruction
        {
            [Tooltip("The world space position to move the platform to")]
            public Vector2 Position;

            [Tooltip("The time to wait (in milliseconds) after the platform reaches its destination")]
            public float WaitTime;

            [Tooltip("The speed at which the platform moves to that position")]            
            public float MovementSpeed;

            [Tooltip("The color to show for the gizmo.  Used for debugging / visualizing paths.")]
            public Color GizmoColor;

        }
        public enum MoveMode
        { 
            [Tooltip("Platform moves positions in order.  After it reaches the last position, it loops back to the first.")]
            Cycle,
            [Tooltip("Platform moves forward through all positions in order.  Once it reaches the last position, it cycles backwards back through the positions")]
            Alternate,
            [Tooltip("Platform chooses a position randomly each time")]
            Random,
            [Tooltip("Platform will only move when the 'CycleNext' method is called externally.")]
            Manual
        }

        private void OnDrawGizmosSelected()
        {
            if (instructions.Any())
            {

                var collider = GetRequiredComponent<Collider2D>();

                foreach (var inst in instructions) {
                    Gizmos.color = inst.GizmoColor;
                    Gizmos.DrawWireCube(inst.Position, collider.bounds.size);
                }          
            }
        }

    }
}

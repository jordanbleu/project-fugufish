using Assets.Source.Math;
using System;
using UnityEngine;

namespace Assets.Source.Components.AI.Base
{
    /// <summary>
    /// This is the driver for all the AI behavior.  Doing it this way allows separate AI behaviors to be totally decoupled 
    /// and independent from each other, and dependencies do not need to be shared among them at all.
    /// </summary>
    public abstract class NPCBrainComponentBase : ComponentBase
    {
        [SerializeField]
        [Tooltip("The state to activate when the actor is idle.")]
        private MonoBehaviour onIdleStateBehavior;

        [SerializeField]
        [Tooltip("The state to activate when the actor is on full alert.")]
        private MonoBehaviour onActiveStateBehavior;

        [SerializeField]
        protected bool useMovementBoundaries = true;

        [SerializeField]
        [Tooltip("Actor will not leave this area (this is the magenta square).")]
        protected Square movementBounds;

        [SerializeField]
        [Tooltip("Drag the player object here.")]
        protected GameObject player;

        public override void ComponentAwake()
        {
            if (!UnityUtils.Exists(onIdleStateBehavior)) {
                throw new UnityException("Idle State Behavior is null.  Please drag one onto the inspector.");
            }

            if (!UnityUtils.Exists(onActiveStateBehavior))
            {
                throw new UnityException("Active State Behavior is null.  Please drag one onto the inspector.");
            }


            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            UpdateState();
            base.ComponentUpdate();
        }

        private void UpdateState()
        {
            if (!useMovementBoundaries || movementBounds.SurroundsPoint(player.transform.position))
            {
                if (!onActiveStateBehavior.isActiveAndEnabled)
                {
                    onActiveStateBehavior.enabled = true;
                }

                if (onIdleStateBehavior.isActiveAndEnabled) 
                {
                    onIdleStateBehavior.enabled = false;
                }
            }
            else 
            {
                if (onActiveStateBehavior.isActiveAndEnabled)
                {
                    onActiveStateBehavior.enabled = false;
                }

                if (!onIdleStateBehavior.isActiveAndEnabled)
                {
                    onIdleStateBehavior.enabled = true;
                }
            }
            
        }

        private void OnDrawGizmosSelected()
        {
            // Draw movement Boundaries
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(movementBounds.Center, new Vector2(movementBounds.Width, movementBounds.Height));
        }
    }
        
}

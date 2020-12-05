using Assets.Source.Components;
using Assets.Source.Components.Physics.Base;
using Assets.Source.Math;
using UnityEngine;

namespace Assets.Source.AI.Base
{
    /// <summary>
    /// Base class for all the brains
    /// </summary>
    public abstract class BrainComponentBase : PhysicsComponentBase
    {
        [Header("Movement Boundaries")]
        [SerializeField]
        private bool useMovementBoundaries = true;
        public bool UseMovementBoundaries { get => useMovementBoundaries; }


        [SerializeField]
        [Tooltip("Actor will not leave this area (this is the magenta square).")]
        private Square movementBounds;
        public Vector2 BoundsX { get => new Vector2(movementBounds.Center.x - (movementBounds.Width / 2), movementBounds.Center.x + (movementBounds.Width / 2)); }
        public Vector2 BoundsY { get => new Vector2(movementBounds.Center.y - (movementBounds.Height / 2), movementBounds.Center.y + (movementBounds.Height / 2)); }

        private void OnDrawGizmosSelected()
        {
            // Draw brain's Boundaries
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(movementBounds.Center, new Vector2(movementBounds.Width, movementBounds.Height));
        }



    }
}

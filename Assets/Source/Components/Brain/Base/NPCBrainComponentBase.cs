using Assets.Source.Components.Actor;
using Assets.Source.Components.MemoryManagement;
using Assets.Source.Math;
using Spine.Unity;
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


        [SerializeField]
        [Tooltip("Used for a stupid hack I had to do.  Set this to the relative height of the sprite / skeleton in unity units (pixels / 16)")]
        private float relativeNpcHeightInUnits = 2f;

        private ActorComponent playerActor;
        protected ActorComponent actor;
        private DespawnComponent despawner;
        private Collider2D collider2d;
        private Rigidbody2D rigidBody;

        private float startingColliderSizeY = 0f;

        public override void ComponentPreStart()
        {
            actor = GetRequiredComponent<ActorComponent>();
            despawner = GetRequiredComponent<DespawnComponent>();
            collider2d = GetRequiredComponent<Collider2D>();
            rigidBody = GetRequiredComponent<Rigidbody2D>();


            if (!UnityUtils.Exists(player)) {
                throw new UnityException($"{gameObject.name} needs a player object dragged onto its NPCBrain");
            }

            playerActor = GetRequiredComponent<ActorComponent>(player);


            if (!UnityUtils.Exists(onIdleStateBehavior)) {
                throw new UnityException("Idle State Behavior is null.  Please drag one onto the inspector.");
            }

            if (!UnityUtils.Exists(onActiveStateBehavior))
            {
                throw new UnityException("Active State Behavior is null.  Please drag one onto the inspector.");
            }


            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            if (actor.IsAlive())
            {
                UpdateState();
            }
            else {
                // Hack to disable collider without disabling gravity 
                if (collider2d is BoxCollider2D boxCollider)
                {
                    boxCollider.size = new Vector2(0.01f, 0.01f);
                    boxCollider.offset = new Vector2(0, -(relativeNpcHeightInUnits / 4));
                }
                else if (collider2d is PolygonCollider2D polygonCollider)
                {
                    polygonCollider.points = new Vector2[] { Vector2.zero };
                }
                else if (collider2d is CapsuleCollider2D capsuleCollider) {
                    capsuleCollider.size = new Vector2(0.01f,0.01f);
                    capsuleCollider.offset = new Vector2(0f, -(relativeNpcHeightInUnits/4));
                }
                                
                despawner.BeginDespawn();
                onActiveStateBehavior.enabled = false;
                onIdleStateBehavior.enabled = false;
            }
            base.ComponentUpdate();
        }


        private void UpdateState()
        {
            if (playerActor.IsAlive())
            {
                var fuckAss = movementBounds.SurroundsPoint(player.transform.position);
                if (!useMovementBoundaries || movementBounds.SurroundsPoint(player.transform.position))
                {
                    DoActiveBehavior();
                }
                else
                {
                    DoIdleBehavior();
                }
            }
            else {
                DoIdleBehavior();
            }
            
        }

        private void DoIdleBehavior()
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

        private void DoActiveBehavior()
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

        private void OnDrawGizmosSelected()
        {
            // Draw movement Boundaries
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(movementBounds.Center, new Vector2(movementBounds.Width, movementBounds.Height));
        }
    }
        
}

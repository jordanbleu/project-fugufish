using Assets.Editor.Attributes;
using Assets.Source.Components.Camera;
using Assets.Source.Components.Interaction;
using UnityEngine;

namespace Assets.Source.Components.Actor
{
    /// <summary>
    /// This component detects attackable components and reports collisions.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class MeleeComponent : ComponentBase
    {
        public bool IsDamageEnabled { get; set; } = false;

        [SerializeField]
        [ReadOnly]
        private bool isDamageEnabled;

        [SerializeField]
        private LayerMask attackableLayers;

        [SerializeField]
        [Tooltip("The physics object that is doing the swinging.  Usually a parent object.  Should probably " +
            "contain a component that inherits from CommonPhysicsComponent (not required though).")]
        private GameObject attacker;

        private LevelCameraEffectorComponent cameraEffector;

        /// <summary>
        /// If true, actor is facing left.  Must be set manually by the actor.
        /// </summary>
        public bool IsFlipped { get; set; } = false;

        public override void ComponentAwake()
        {
            cameraEffector = GetRequiredComponent<LevelCameraEffectorComponent>(GetRequiredObject("Level"));
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // Display in inspector
            isDamageEnabled = IsDamageEnabled;

            // Flip the collider along with the player
            if (!IsFlipped)
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
            }

            base.ComponentUpdate();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (IsDamageEnabled) {
                if (attackableLayers.IncludesLayer(collision.gameObject.layer) && 
                    collision.gameObject.TryGetComponent<AttackableComponent>(out var attackable)) {

                    cameraEffector.Impact();

                    attackable.Attack(attacker);

                    if (attackable.EndsAttackAnimation) {
                        IsDamageEnabled = false;
                    }   
                }                
            }            
        }

    }
}

using Assets.Source.Components.Actor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    [RequireComponent(typeof(Collider2D), typeof(ActorComponent))]
    public class DestructibleComponent : ComponentBase
    {
        private ActorComponent actor;

        [SerializeField]
        [Header("Drag the player's attack collider object here")]
        private GameObject playerAttackObject;

        public override void ComponentAwake()
        {
            actor = GetRequiredComponent<ActorComponent>();
            base.ComponentAwake();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Checks to see if we collided with the player's swing collider
            if (collision.gameObject.Equals(playerAttackObject)) {
                // If the player is at the proper moment in their attack animation, invoke the delegate
                if (collision.gameObject.TryGetComponent<PlayerSwingColliderComponent>(out var swingCollider) && swingCollider.IsActive) {
                    // A destructible object is always depleted by 1
                    actor.DepleteHealth(1);                    
                }
            }

            
        }



    }
}

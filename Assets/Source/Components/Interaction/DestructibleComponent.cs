using Assets.Source.Components.Actor;
using Assets.Source.Components.Brain;
using UnityEngine;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// A destructible component reacts to being hit by the player.
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(ActorComponent))]
    public class DestructibleComponent : ComponentBase
    {
        private ActorComponent actor;

        [SerializeField]
        [Tooltip("if true, will end a player's attack on contact")]
        private bool stopsAttack = true;

        public override void ComponentPreStart()
        {
            actor = GetRequiredComponent<ActorComponent>();
            base.ComponentPreStart();
        }

        //todo: uh why is this commented lol
        private void OnTriggerStay2D(Collider2D collision)
        {
            //// Checks to see if we collided with the player's swing collider
            //if (collision.gameObject.TryGetComponent<PlayerSwingColliderComponent>(out var swingCollider)) {

            //    var playerComponent = collision.gameObject.GetComponentInParent<PlayerBrainComponent>();

            //    if (playerComponent == null) {
            //        throw new UnityException("Found a swing collider component with no player physics component in the parent");
            //    }

            //    if (playerComponent.IsDamageEnabled) { 
            //        // A destructible object is always depleted by 1
            //        actor.DepleteHealth(1);

            //        if (stopsAttack) { 
            //            playerComponent.OnAttackEnd();
            //            playerComponent.OnDamageDisable();
            //        }
            //    }
            //}
            

            
        }



    }
}

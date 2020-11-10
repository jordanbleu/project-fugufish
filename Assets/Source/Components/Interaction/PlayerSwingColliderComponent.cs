using Assets.Source.Components.Player;
using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// Handles player's melee attack logic and animations
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class PlayerSwingColliderComponent : ComponentBase    {

        private PlayerPhysicsComponent player;

        public override void ComponentAwake()
        {
            player = GetRequiredComponentInParent<PlayerPhysicsComponent>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            // follow the scale of the parent's skeleton
            var isFlipped = (player.SkeletonScale < 0);

            // this is painfully stupid 
            if (!isFlipped)
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
            else {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
            }

            base.ComponentUpdate();
        }

    }
}

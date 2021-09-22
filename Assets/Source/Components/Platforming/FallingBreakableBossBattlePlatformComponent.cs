using Assets.Source.Components.Behavior.Humanoid;
using Assets.Source.Components.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Platforming
{
    /// <summary>
    /// When destroyed, this component will enable gravity, making the object fall downwards.  When it hits the boss,
    /// it will destroy itself and hurt the boss.
    /// </summary>
    public class FallingBreakableBossBattlePlatformComponent : ComponentBase
    {
        private Rigidbody2D rigidBody;
        private bool hasFallen = false;


        [SerializeField]
        private UnityEvent onFallOnSolid = new UnityEvent();

        [SerializeField]
        private LayerMask explodeOnLayer = new LayerMask();

        public override void ComponentPreStart()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentPreStart();
        }

        /// <summary>
        /// Triggers the falling animation
        /// </summary>
        public void Fall() {
            rigidBody.constraints = RigidbodyConstraints2D.None;
            hasFallen = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasFallen && collision.gameObject.TryGetComponent<AttackableComponent>(out var comp))
            {
                // if it lands on the boss trigger the boss thing
                if (comp.gameObject.name != "Player")
                {
                    comp.Attack(gameObject);
                    onFallOnSolid?.Invoke();

                    // Boss gets stunned by falling objects
                    if (collision.gameObject.TryGetComponent<BossEnemyBehavior>(out var behavior)) {
                        behavior.Stun();
                    }

                }
            }
            else if (explodeOnLayer.IncludesLayer(collision.gameObject.layer)) {
                onFallOnSolid?.Invoke();
            }
        }
    }
}

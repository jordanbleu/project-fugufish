using Assets.Source.Components.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Projectile
{
    /// <summary>
    /// All this does is move the projectile in a direction
    /// not sure we need this.
    /// </summary>
    public class EnemyBulletComponent : ComponentBase
    {

        [SerializeField]
        private LayerMask attackableLayers;

        public override void ComponentStart()
        {
            if (attackableLayers.IsEverything() || attackableLayers.IsNothing())
            {
                Debug.LogWarning($"Object '{gameObject.name}' / Component '{GetType().Name}' has an 'attackableLayers' layermask " +
                    $"of everything or nothing which probably isnt right");
            }
            base.ComponentStart();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != gameObject && attackableLayers.IncludesLayer(collision.gameObject.layer))
            {
                if (collision.gameObject.TryGetComponent<AttackableComponent>(out var attackable)) { 
                    attackable.Attack(gameObject);             
                }
                Destroy(gameObject);
            }

            
        }

    }
}

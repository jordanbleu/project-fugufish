using Assets.Source.Components.Brain;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// GameObjects with this component can be attacked, and react to it.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class AttackableComponent : ComponentBase
    {
        [SerializeField]
        private UnityEvent onPlayerAttack;

        [SerializeField]
        private UnityEvent onNpcAttack;

        [SerializeField]
        [Tooltip("Signals to the attacking actor to stop their attack animation when they hit this object.")]
        private bool endsAttackAnimation = true;
        public bool EndsAttackAnimation { get => endsAttackAnimation; }


        /// <summary>
        /// Called when we were attacked by someone!
        /// </summary>
        public void Attack(GameObject attacker) {

            if (UnityUtils.Exists(gameObject) && UnityUtils.Exists(this) && gameObject.activeInHierarchy)
            {
                if (attacker.TryGetComponent<PlayerBrainComponent>(out var player))
                {
                    onPlayerAttack?.Invoke();
                }
                else
                {
                    onNpcAttack?.Invoke();
                }
            }
        }


    }
}

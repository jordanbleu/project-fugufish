using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Actor
{
    public class ActorComponent : ComponentBase
    {

        [SerializeField]
        private int _health = 1;

        
        [SerializeField]
        [Tooltip("This will be invoked each time the actor's health gets depleted by any amount")]
        private UnityEvent onHealthDamage;

        
        [SerializeField]
        [Tooltip("This will be invoked on each frame that the actor has no health, so this should usually handle destroying the component")]
        private UnityEvent onHealthEmpty;
        public int Health { get => _health; }

        public bool IsAlive() => Health > 0;


        /// <summary>
        /// The proper way to decrease the actor's health
        /// </summary>
        /// <param name="amount">The amount to deplete health by</param>
        public void DepleteHealth(int amount) {
            onHealthDamage?.Invoke();
            _health -= amount;
        }

        public override void ComponentUpdate()
        {
            if (!IsAlive()) {
                onHealthEmpty?.Invoke();                
            }

            base.ComponentUpdate();
        }

    }
}

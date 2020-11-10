using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Actor
{
    public class ActorComponent : ComponentBase
    {
        [SerializeField]
        private int _maxHealth = 100;
        public int MaxHealth { get => _maxHealth; }

        
        [SerializeField]
        [Tooltip("This will be invoked each time the actor's health gets depleted by any amount")]
        private UnityEvent onHealthDamage;

        
        [SerializeField]
        [Tooltip("This will be invoked on each frame that the actor has no health, so this should usually handle destroying the component")]
        private UnityEvent onHealthEmpty;

        public int Health { get; private set; }

        public bool IsAlive() => Health > 0;

        public override void ComponentStart()
        {
            Health = _maxHealth;
            base.ComponentAwake();
        }

        /// <summary>
        /// The proper way to decrease the actor's health
        /// </summary>
        /// <param name="amount">The amount to deplete health by</param>
        public void DepleteHealth(int amount) {
            onHealthDamage?.Invoke();
            Health -= amount;
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

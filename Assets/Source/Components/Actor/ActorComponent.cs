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
        private UnityEvent onHealthDepleted;

        [SerializeField]
        private string DebugString;

        private bool isAlive = true;

        public int Health { get; set; }

        public override void ComponentAwake()
        {
            Health = _maxHealth;
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            base.ComponentUpdate();
        }

    }
}

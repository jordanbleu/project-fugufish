using Assets.Editor.Attributes;
using Assets.Source.Components.Timer;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Actor
{
    public class ActorComponent : ComponentBase
    {

        [Header("Health")]
        [SerializeField]
        [ReadOnly]
        private int health;
        public int Health { get => health;  set => health = value; }

        [SerializeField]
        private int maxHealth;
        public int MaxHealth { 
            get => maxHealth; 
            set => maxHealth = value;
        } 

        [Tooltip("This will be invoked each time the actor's health gets depleted by any amount")]
        public UnityEvent onHealthDamage = new UnityEvent();


        [Tooltip("Invoked when you call Deplete health and the actor is out of health.")]
        public UnityEvent onHealthEmpty = new UnityEvent();


        [Header("Stamina")]
        [SerializeField]
        [ReadOnly]
        private int stamina;
        public int Stamina { get => stamina; set => stamina = value; }

        [SerializeField]
        private int maxStamina;
        public int MaxStamina { get => maxStamina; }

        [SerializeField]
        [Tooltip("Whether or not to auto refill stamina.")]
        private bool refillStamina = true;

        [SerializeField]
        [Tooltip("How quickly stamina refills in milliseconds.  Stamina will go up by 1 with each interval.")]
        private int staminaRefillDelay = 50;


        [Tooltip("Used when you try to deplete stamina, and still had enough stamina left.")]
        public UnityEvent onStaminaDepleted = new UnityEvent();

        [Tooltip("Invoked when you try to deplete stamina and you don't have enough stamina left.")]
        public UnityEvent onStaminaEmpty = new UnityEvent();

        [SerializeField]
        [Header("Cheats")]
        private bool infiniteHealth = false;

        private IntervalTimerComponent staminaTimer;

        public bool IsAlive() => Health > 0;

        /// <summary>
        /// The proper way to decrease the actor's health
        /// </summary>
        /// <param name="amount">The amount to deplete health by</param>
        public void DepleteHealth(int amount)
        {
            if (!infiniteHealth)
            {
                if (Health > 0)
                {
                    onHealthDamage?.Invoke();

                    if (Health >= amount)
                    {
                        Health -= amount;
                    }
                    else {
                        Health = 0;
                    }

                    if (Health <= 0)
                    {
                        onHealthEmpty?.Invoke();
                        Health = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Increases the actors health without going over max
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseHealth(int amount) {
            Health += amount;

            if (Health > MaxHealth) {
                Health = MaxHealth;
            }
        }

        /// <summary>
        /// Takes the passed in amount and returns true if there's enough stamina to cover the requested amount.  Returns 
        /// false if not, and does nothing. 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryDepleteStamina(int amount)
        {
            if (stamina >= amount)
            {
                onStaminaDepleted?.Invoke();
                stamina -= amount;
                return true;
            }
            onStaminaEmpty?.Invoke();
            return false;
        }

        public override void ComponentAwake()
        {
            Health = MaxHealth;
            stamina = MaxStamina;

            if (!UnityUtils.Exists(staminaTimer) && refillStamina)
            {
                var staminaTimerPrefabTemp = new GameObject("Stamina Timer Object (temp)");
                staminaTimerPrefabTemp.AddComponent<IntervalTimerComponent>();

                var instance = Instantiate(staminaTimerPrefabTemp, transform);

                staminaTimer = GetRequiredComponent<IntervalTimerComponent>(instance);
                staminaTimer.Label = "StaminaTimerComponent";
                staminaTimer.SetInterval(staminaRefillDelay);
                staminaTimer.SelfDestruct = false;
                staminaTimer.Randomize = false;
                staminaTimer.AutoReset = true;
                staminaTimer.OnIntervalReached.AddListener(RefillStaminaIntervalReached);
                staminaTimer.IsActive = true;

                instance.name = "StaminaTimer";

                Destroy(staminaTimerPrefabTemp);
            }
            base.ComponentAwake();
        }

        private void RefillStaminaIntervalReached()
        {
            if (stamina < maxStamina)
            {
                stamina++;
            }
        }

        /// <summary>
        /// Invoke this to make the actor instantly take full damage 
        /// </summary>
        public void DieInstantly() => DepleteHealth(MaxHealth);

        /// <summary>
        /// Call this from one of the unity events to spawn a prefab at the current position of the actor
        /// </summary>
        /// <param name="prefab"></param>
        public void SpawnItemAtMyPosition(GameObject prefab) {
            var inst = Instantiate(prefab, transform.parent);
            inst.transform.position = transform.position;
        }
    }
}

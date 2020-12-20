using Assets.Source.Components.AI.Base;
using Assets.Source.Components.Brain.Interfaces;
using Assets.Source.Enums;
using System;
using UnityEngine.Events;

namespace Assets.Source.Components.Brain
{
    public class HumanoidNPCBrainComponent : NPCBrainComponentBase, IHumanoidBrain
    {
        // Unity Events
        [NonSerialized]
        public UnityEvent attackByPlayer = new UnityEvent();
        [NonSerialized]
        public UnityEvent attackBegin = new UnityEvent();
        [NonSerialized]
        public UnityEvent attackEnd = new UnityEvent();
        [NonSerialized]
        public UnityEvent damageEnable = new UnityEvent();
        [NonSerialized]
        public UnityEvent damageDisable = new UnityEvent();
        [NonSerialized]
        public UnityEvent groundPoundBegin = new UnityEvent();
        [NonSerialized]
        public UnityEvent groundPoundLanded = new UnityEvent();
        [NonSerialized]
        public UnityEvent upperCutBegin = new UnityEvent();

        private bool damageEnableFlag = false;


        private PlayerBrainComponent playerBrain;

        public override void ComponentAwake()
        {
            base.ComponentAwake();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
        }

        public override void ComponentFixedUpdate()
        {
            // Ensures that animation events are only emitted once per physics update
            damageEnableFlag = false;

            base.ComponentFixedUpdate();
        }

        public void OnAttackedByPlayer() => attackByPlayer?.Invoke();

        #region Animation Events
        public void OnAttackBegin() => attackBegin?.Invoke();

        public void OnAttackEnd() => attackEnd?.Invoke();

        public void OnDamageDisable() => damageDisable?.Invoke();

        public void OnDamageEnable() {
            if (damageEnableFlag) { 
                damageEnable?.Invoke(); 
            }
        }

        public void OnGroundPoundBegin() => groundPoundBegin?.Invoke();

        public void OnGroundPoundLanded() => groundPoundLanded?.Invoke();

        public void OnUppercutBegin() => upperCutBegin?.Invoke();
        #endregion
    }
}

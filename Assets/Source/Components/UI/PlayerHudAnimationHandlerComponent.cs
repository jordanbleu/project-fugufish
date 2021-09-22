using Assets.Source.Components.Actor;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.UI
{
    public class PlayerHudAnimationHandlerComponent : ComponentBase
    {

        private enum FlashAlertLevel 
        {
            OK =0,
            LowHealth = 1,
            LowStamina =2,
            LowBoth = 3        
        }


        private ActorComponent playerActor;
        private Animator animator;
        
        [SerializeField]
        [Tooltip("The percent of max health to begin flashing the health bar")]
        [Range(0,1)]
        private float lowHealthThreshold = 0.25f;

        public override void ComponentPreStart()
        {
            animator = GetRequiredComponent<Animator>();
            playerActor = GetRequiredComponent<ActorComponent>(GetRequiredObject("Player"));
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            var lowHealth = playerActor.Health <= (playerActor.MaxHealth * lowHealthThreshold);
            var lowStam = playerActor.Stamina <= (playerActor.MaxStamina * lowHealthThreshold);

            // todo:  Bitshifting
            
            if (!lowHealth && !lowStam)
            {
                animator.SetFloat("hud_alert_level", 0);
            }
            else if (lowHealth && !lowStam)
            {
                animator.SetFloat("hud_alert_level", 1);
            }
            else if (!lowHealth && lowStam)
            {
                animator.SetFloat("hud_alert_level", 2);
            }
            else if (lowHealth && lowStam) {
                animator.SetFloat("hud_alert_level", 3);
            }

            base.ComponentUpdate();
        }

    }
}

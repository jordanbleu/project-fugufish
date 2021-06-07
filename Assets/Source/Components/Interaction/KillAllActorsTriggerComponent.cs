using Assets.Source.Components.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    public class KillAllActorsTriggerComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("The Enemies that must be killed")]
        List<ActorComponent> actorsToKill;

        [SerializeField]
        [Tooltip("If true, the event will be invoked every frame.")]
        private bool isContinuous;

        [SerializeField]
        private UnityEvent onAllActorsKilled;

        private bool wasTriggered = false;

        public override void ComponentUpdate()
        {
            if (isContinuous || !wasTriggered)
            {
                if (!actorsToKill.Any(actor => UnityUtils.Exists(actor) && actor.IsAlive()))
                {
                    onAllActorsKilled?.Invoke();
                    wasTriggered = true; 
                }
            }
            base.ComponentUpdate();
        }

    }
}

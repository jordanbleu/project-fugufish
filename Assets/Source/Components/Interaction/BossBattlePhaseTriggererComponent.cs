using Assets.Source.Components.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// Handles triggering things throughout the boss battle
    /// </summary>
    public class BossBattlePhaseTriggererComponent : ComponentBase
    {

        [SerializeField]
        private List<TriggerInstruction> instructions;

        [SerializeField]
        private ActorComponent bossActor;
        private Queue<TriggerInstruction> instructionQueue;
        private TriggerInstruction? currentInstruction;

        public override void ComponentAwake()
        {
            // create an ordered queue so we don't have to loop through all instructions each time
            // This is ordered by boss health going highest > lowest 
            instructionQueue = new Queue<TriggerInstruction>(instructions.OrderByDescending(i => i.BossHealth));
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if ((currentInstruction != null && currentInstruction.HasValue))
            {
                if (bossActor.Health <= currentInstruction.Value.BossHealth)
                {
                    currentInstruction.Value.Event?.Invoke();
                    DequeueNextInstruction();
                }
            }
            else {
                DequeueNextInstruction();
            }
            
            base.ComponentUpdate();
        }

        private void DequeueNextInstruction()
        {
            if (instructionQueue.Any())
            {
                currentInstruction = instructionQueue.Dequeue();
            }
            else
            {
                // we have done all that is needed so we destroy ourselves for valhalla
                Destroy(gameObject);
            }
        }




        // These are defined in the inspector        
        [Serializable]
        public struct TriggerInstruction {

            [Tooltip("When the boss health gets to this point, do the thing")]
            public float BossHealth;

            [Tooltip("What to do when the health gets here")]
            public UnityEvent Event;

        }
    }
}

using Assets.Source.Components.Actor;
using Assets.Source.Components.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Components.Player
{
    public class DepleteHealthOverTimeComponent : ComponentBase
    {
        private ActorComponent actorComponent;
        private IntervalTimerComponent intervalTimerComponent;
        

        public override void ComponentAwake()
        {
            intervalTimerComponent = GetRequiredComponent<IntervalTimerComponent>(GetRequiredChild("HealthDepletionInterval"));
            intervalTimerComponent.OnIntervalReached.AddListener(DepleteHealth);

            actorComponent = GetRequiredComponent<ActorComponent>();
            base.ComponentAwake();
        }

        private void DepleteHealth()
        {
            actorComponent.Health -= 1;
        }
    }
}

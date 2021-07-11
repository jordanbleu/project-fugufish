using Assets.Source.Scene;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class DeathMarkerComponent : ComponentBase
    {
        
        public override void ComponentStart()
        {
            transform.position = GameDataTracker.LastGroundedDeathPosition.GetValueOrDefault();
            base.ComponentStart();
        }

    }
}

using Assets.Source.Components.Actor;
using Assets.Source.Components.Collectibles.Base;
using UnityEngine;

namespace Assets.Source.Components.Collectibles
{
    public class AdrenalineCollectibleComponent : CollectibleComponentBase
    {

        [SerializeField]
        [Tooltip("How much health will be increased by")]
        private int healthIncrease;

        public override void ItemCollected()
        {
            var actor = GetRequiredComponent<ActorComponent>(playerObject);

            if (UnityUtils.Exists(actor)) {
                actor.IncreaseHealth(healthIncrease);
            }
            
        }
    }
}

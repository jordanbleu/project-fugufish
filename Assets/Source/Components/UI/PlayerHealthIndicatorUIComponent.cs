using Assets.Source.Components.Actor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class PlayerHealthIndicatorUIComponent : ComponentBase
    {
        private ActorComponent playerActor;
        private GameObject healthBar;
        private Image healthBarImage;

        public override void ComponentPreStart()
        {
            playerActor = GetRequiredComponent<ActorComponent>(GetRequiredObject("Player"));
            healthBarImage = GetRequiredComponent<Image>(GetRequiredChild("Health"));

            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            if (playerActor != null && healthBarImage != null) {
                healthBarImage.transform.localScale = new Vector2(playerActor.Health / (float)playerActor.MaxHealth, 1);
            }
            base.ComponentUpdate();
        }

    }
}

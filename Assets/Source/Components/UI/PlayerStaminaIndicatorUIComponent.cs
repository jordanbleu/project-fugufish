using Assets.Source.Components.Actor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class PlayerStaminaIndicatorUIComponent : ComponentBase
    {
        private ActorComponent playerActor;
        private GameObject stamBar;
        private Image stamBarImage;

        public override void ComponentPreStart()
        {
            playerActor = GetRequiredComponent<ActorComponent>(GetRequiredObject("Player"));
            stamBarImage = GetRequiredComponent<Image>(GetRequiredChild("Stamina"));
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            if (playerActor != null && stamBarImage != null)
            {
                stamBarImage.transform.localScale = new Vector2(playerActor.Stamina / (float)playerActor.MaxStamina, 1);
            }
            base.ComponentUpdate();
        }
    }
}

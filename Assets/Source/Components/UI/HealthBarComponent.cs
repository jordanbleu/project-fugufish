using Assets.Source.Components.Actor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class HealthBarComponent : ComponentBase
    {
        private Image healthBarImage;
        private ActorComponent playerActor;

        private float fullBarWidth;

        private RectTransform rectTransform;

        public override void ComponentAwake()
        {
            playerActor = GetRequiredComponent<ActorComponent>(GetRequiredObject("Player"));
            healthBarImage = GetRequiredComponent<Image>(GetRequiredChild("HealthBarFG"));

            fullBarWidth = healthBarImage.preferredWidth;

            rectTransform = healthBarImage.rectTransform;
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            var healthPercent = (float)playerActor.Health / playerActor.MaxHealth;

            rectTransform.sizeDelta = new Vector2(fullBarWidth * healthPercent, rectTransform.sizeDelta.y);

            base.ComponentUpdate();
        }
    }
}

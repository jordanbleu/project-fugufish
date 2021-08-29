using Assets.Source.Components.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class BossHealthInidicatorUIComponent : ComponentBase
    {
        [SerializeField]
        private ActorComponent actor;

        [SerializeField]
        private Image healthBarImage;

        [SerializeField]
        [Tooltip("Kinda hacky...This adds a static multiplayer for stretched health bar images. ")]
        private float barWidthMultiplier =1f;


        public override void ComponentUpdate()
        {
            if (actor != null && healthBarImage != null)
            {
                healthBarImage.transform.localScale = new Vector2(barWidthMultiplier * (actor.Health / (float)actor.MaxHealth), healthBarImage.transform.localScale.y);
            }
            base.ComponentUpdate();
        }

    }
}

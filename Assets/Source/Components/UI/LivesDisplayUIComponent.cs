using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Source.Components.UI
{
    public class LivesDisplayUIComponent : ComponentBase
    {
        private TextMeshProUGUI textMesh;

        public override void ComponentPreStart()
        {
            textMesh = GetRequiredComponent<TextMeshProUGUI>();
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            textMesh.SetText($"{GameDataTracker.Lives} / 3");
            base.ComponentUpdate();
        }

    }
}

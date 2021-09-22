using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class BloodSampleHudDisplayComponent : ComponentBase
    {       
        private Animator animator;

        [SerializeField]
        private TextMeshProUGUI textMesh;

        public override void ComponentPreStart()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentPreStart();
        }


        public override void ComponentUpdate()
        {
            textMesh.SetText($"{GameDataTracker.CurrentBloodSamples} / {GameDataTracker.TotalBloodSamples}");
            base.ComponentUpdate();
        }

        public void ShowHud() => animator.SetTrigger("show");

    }
}

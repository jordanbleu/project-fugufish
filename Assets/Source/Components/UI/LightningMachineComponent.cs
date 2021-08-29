using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class LightningMachineComponent : ComponentBase
    {
        [SerializeField]
        private Animator lightningAnimator;
        public void Flash() 
        {
            lightningAnimator.SetTrigger("flash");      
        }

    }
}

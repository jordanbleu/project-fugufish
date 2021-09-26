using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class LightningMachineComponent : ComponentBase
    {
        [SerializeField]
        private Animator lightningAnimator;

        private AudioSource audioSource;

        [SerializeField]
        private AudioClip thunder;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentAwake();
        }

        public void Flash() 
        {
            audioSource.PlayOneShot(thunder);
            lightningAnimator.SetTrigger("flash");      
        }

    }
}

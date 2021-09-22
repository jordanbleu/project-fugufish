using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Sound
{
    public class FootstepAudioComponent : ComponentBase
    {        
        private AudioSource audioSource;

        public override void ComponentPreStart()
        {
            audioSource = GetRequiredComponent<AudioSource>();

            base.ComponentPreStart();
        }

        [SerializeField]
        private AudioClip leftFootstep;

        [SerializeField]
        private AudioClip rightFootstep;

        private int step = 1;

        public void DoTippyTap() {
            
            if (step > 0)
            {
                audioSource.PlayOneShot(leftFootstep);
            }
            else 
            {
                audioSource.PlayOneShot(rightFootstep);
            }

            step = -step;
        }
    }
}

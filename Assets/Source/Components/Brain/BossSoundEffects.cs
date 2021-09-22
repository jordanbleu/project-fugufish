using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    public class BossSoundEffects : ComponentBase
    {

        [SerializeField]
        private AudioClip footStep1;

        [SerializeField]
        private AudioClip footStep2;

        [SerializeField]
        private AudioClip groundSmash;

        [SerializeField]
        private AudioClip stunHit;

        [SerializeField]
        private AudioClip swordSwing;

        private AudioSource audioSource;

        public override void ComponentPreStart()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentPreStart();
        }

        public void PlayFootstep1() => audioSource.PlayOneShot(footStep1);

        public void PlayFootstep2() => audioSource.PlayOneShot(footStep2);

        public void PlayGroundSmash() => audioSource.PlayOneShot(groundSmash);

        public void PlayStunHit() => audioSource.PlayOneShot(stunHit);

        public void PlaySwordSwing() => audioSource.PlayOneShot(swordSwing);

        






    }
}

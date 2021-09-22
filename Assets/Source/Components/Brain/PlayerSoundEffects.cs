using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    public class PlayerSoundEffects : ComponentBase
    {
        private AudioSource audioSource;

        public override void ComponentPreStart()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentPreStart();
        }

        [SerializeField]
        private AudioClip swordSwing0;

        [SerializeField]
        private AudioClip swordSwing1;

        [SerializeField]
        private AudioClip swordSwing2;

        [SerializeField]
        private AudioClip ladderClink1;

        [SerializeField]
        private AudioClip ladderClink2;

        [SerializeField]
        private AudioClip roll;

        public void Swing0() => audioSource.PlayOneShot(swordSwing0);
        
        public void Swing1() => audioSource.PlayOneShot(swordSwing1);

        public void Swing2() => audioSource.PlayOneShot(swordSwing2);

        public void LadderClink1() => audioSource.PlayOneShot(ladderClink1);

        public void LadderClink2() => audioSource.PlayOneShot(ladderClink2);

        public void Roll() => audioSource.PlayOneShot(roll);

    }
}

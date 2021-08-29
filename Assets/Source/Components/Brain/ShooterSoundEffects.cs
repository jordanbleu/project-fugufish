using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    public class ShooterSoundEffects : ComponentBase
    {


        [SerializeField]
        private AudioClip shot1;

        private AudioSource audioSource;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentAwake();
        }


        public void Shot1() => audioSource.PlayOneShot(shot1);

    }
}

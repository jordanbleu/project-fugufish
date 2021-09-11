﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Sound
{
    /// <summary>
    /// TODO: a better way might be https://answers.unity.com/questions/17856/sound-stops-when-object-is-destroyed.html
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class OneTimeSoundSourceComponent : ComponentBase
    {
        private AudioSource audioSource;
        private bool isActivated = false;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (isActivated) {
                if (!audioSource.isPlaying) {
                    Destroy(gameObject);
                }
            }
            base.ComponentUpdate();
        }

        public void PlaySound(AudioClip clip) {
            isActivated = true;
            audioSource.PlayOneShot(clip);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Sound
{
    [RequireComponent(typeof(Animator))]
    public class MusicBoxComponent : ComponentBase
    {

        private Animator animator;
        private AudioSource audioSource;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            audioSource = GetRequiredComponent<AudioSource>();
            base.ComponentAwake();
        }

        public void FadeInAndPlay() {
            animator.SetTrigger("fade_in");
        }

        public void FadeOutAndStop() {
            animator.SetTrigger("fade_out");
        }

        public void Play() {
            OnPlay();
            animator.SetTrigger("play");
        }

        public void Stop() {
            OnStop();
            animator.SetTrigger("stop");
        }
             

        public void OnPlay() {
            audioSource.Play();
        }

        public void OnStop() {
            audioSource.Stop();
        }




    }
}

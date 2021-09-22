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
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private AudioSource audioSource;


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

        public void SetAudio(AudioClip clip) {
            audioSource.clip = clip;
        }



    }
}

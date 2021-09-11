using Assets.Source.Components.Sound;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class TitleScreenComponent : ComponentBase
    {

        [SerializeField]
        private AudioClip enterSound;

        private MusicBoxComponent music;
        private MusicBoxComponent ambience;

        private Animator animator;
        private AudioSource audioSource;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            audioSource = GetRequiredComponent<AudioSource>();

            music = GetRequiredComponent<MusicBoxComponent>(GetRequiredObject("Music"));
            ambience = GetRequiredComponent<MusicBoxComponent>(GetRequiredObject("RainAmbience"));

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (UnityEngine.Input.anyKey) {
                animator.SetTrigger("close");                
            }
            base.ComponentUpdate();
        }

        public void OnTitleScreenFadeOut() {
            Destroy(gameObject);
        }

        public void OnKeyPressed() {
            audioSource.PlayOneShot(enterSound);
            music.FadeOutAndStop();
            ambience.FadeOutAndStop();
        }


    }
}

using UnityEngine;

namespace Assets.Source.Components.Cutscenes.Rooftop
{
    public class RooftopCutsceneDirector : ComponentBase
    {


        [SerializeField]
        private Animator fadeAnimator;


        public override void ComponentStart()
        {
            fadeAnimator.SetTrigger("fade_in");
            base.ComponentStart();
        }

        public void OnFadeInCompleted() {
            Destroy(gameObject);
        }

    }
}

using UnityEngine;

namespace Assets.Source.Components.Interaction
{
    [RequireComponent(typeof(Animator))]
    public class DoorAnimatorComponent : ComponentBase
    {

        // Animator should handle pretty much all of the logic
        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }


        public void Open() {
            animator.SetBool("is_open", true);
        }

        public void Close() {
            animator.SetBool("is_open", false);
        }

    }
}

using UnityEngine;

namespace Assets.Source.Components.Objects
{
    /// <summary>
    /// These doors don't transition anywhere.  They're just a solid that is in the way until opened.
    /// </summary>
    public class DoorComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("Doesn't work in real time currently")]
        private bool isOpen = false;

        private Animator animator;
        private Collider2D collider2d;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            collider2d = GetRequiredComponent<Collider2D>();
            base.ComponentAwake();
        }

        public void Toggle() {
            isOpen = !isOpen;
            animator.SetBool("is_open", isOpen);
        }

        // Called from anim event
        public void OnDoorOpen() {
            collider2d.enabled = false;
        }

        // Called from anim event
        public void OnDoorClose() {
            collider2d.enabled = true;
        }


    }
}

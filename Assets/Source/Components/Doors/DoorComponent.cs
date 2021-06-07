using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Objects
{
    /// <summary>
    /// This component can be used for any basic door, with the following requirements:
    /// <list type="bullet">
    /// <item>A collider on the object that will act as the doors barrier (and will be disabled when the door opens)</item>
    /// <item>An animator that responds to an "is_open" parameter, and also disable the collider when the door is open</item>
    /// </list>
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class DoorComponent : ComponentBase
    {

        [SerializeField]
        private UnityEvent onDoorOpening = new UnityEvent();

        [SerializeField]
        private UnityEvent onDoorClosing = new UnityEvent();

        [SerializeField]
        [Tooltip("Whether the door starts open or closed (doesn't work in editor in real time)")]
        private bool isOpen = false;
        public bool IsOpen { get => isOpen; }


        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        public void Toggle() {
            isOpen = !isOpen;
            animator.SetBool("is_open", isOpen);

            if (isOpen)
            {
                onDoorClosing?.Invoke();
            }
            else {
                onDoorOpening?.Invoke();
            }

        }

        public void OnDoorClose() { }


    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Objects
{
    [Obsolete("Don't use this, it is dumb code")]
    public class DoubleDoorComponent : ComponentBase
    {

        [SerializeField]
        private UnityEvent onEnterOpenDoor = new UnityEvent();

        private bool isOpen = false;
        private Animator animator;
        private bool wasTriggered = false;

        public override void ComponentPreStart() {
            animator = GetRequiredComponent<Animator>();
            base.ComponentPreStart();
        }

        public void Open() => animator.SetBool("is_open", true);
        
        public void Close() => animator.SetBool("is_open", false);

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!wasTriggered)
            {
                if (isOpen)
                {
                    wasTriggered = true;
                    onEnterOpenDoor?.Invoke();
                }
            }
        }



        // Called from animation timeline
        public void OnDoorOpenOrClosed() => isOpen = !isOpen;
               
    }
}

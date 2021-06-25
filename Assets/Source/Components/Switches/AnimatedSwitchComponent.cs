using Assets.Source.Components.Switches.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Switches
{
    /// <summary>
    /// Used for a lever switch that has an animator controller instead of simply 2 sprites.
    /// </summary>
    public class AnimatedSwitchComponent : ComponentBase
    {


        [SerializeField]
        [Tooltip("Event that gets invoked when the switch is turned to on position.  Ignored if isToggle is false.")]
        private UnityEvent onSwitchTurnOn = new UnityEvent();

        [SerializeField]
        [Tooltip("Event that gets invoked when the switch is turned to off position.  Ignored if isToggle is false.")]
        private UnityEvent onSwitchTurnOff = new UnityEvent();


        [SerializeField]
        [Tooltip("If true, switch can be turned on or off.  Otherwise works like a button.")]
        private bool isToggle = false;

        [SerializeField]
        private bool isOn;

        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }


        /// <summary>
        /// Display the animation for flipping the switch on / off (like a slot machine kinda).
        /// In the attackable component you should also trigger the action you want the switch to do.
        /// </summary>
        public void ShowSwitchAnimation() {
            if (!isToggle)
            {
                animator.SetTrigger("toggle_on_off");
            }
            else {
                // Toggle
                isOn = !isOn;

                // Invoke Events
                if (isOn)
                {
                    onSwitchTurnOn?.Invoke();
                }
                else {
                    onSwitchTurnOff?.Invoke();
                }

                // Animate
                animator.SetBool("is_on", isOn);
            }
        }






    }
}

using Assets.Source.Components.Switches.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Switches
{
    /// <summary>
    /// Used for a lever switch that has an animator controller instead of simply 2 sprites.
    /// </summary>
    public class AnimatedSwitchComponent : ComponentBase
    {
        

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
        public void ShowTriggerOnOffAnimation() {
            animator.SetTrigger("toggle_on_off");
        }





    }
}

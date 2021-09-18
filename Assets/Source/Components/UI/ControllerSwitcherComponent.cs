using Assets.Source.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class ControllerSwitcherComponent : ComponentBase
    {

        [SerializeField]
        private string ass;
        public override void ComponentUpdate()
        {

            ass = UnityEngine.Input.GetAxis("RightTrigger").ToString();

            if (Input.GetActiveListener().GetType() == typeof(GamepadInputListener))
            {
                if (UnityEngine.Input.GetKey(KeyCode.Space))
                {
                    Input.SwapInputModes(new KeyboardInputListener());
                    Debug.Log("Swapped to keyboard input");
                }
            }
            else {
                if (UnityEngine.Input.GetButton("A_Button")) {
                    Input.SwapInputModes(new GamepadInputListener());
                    Debug.Log("Swapped to gamepad input");
                }
            }

            base.ComponentUpdate();
        }
    }
}

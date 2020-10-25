using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Input
{
    public class KeyCodeValue
    {
        public enum AxisDirections
        {
            Positive = 0,
            Negative = 1
        }

        /// <summary>
        /// This is the actual key code or input id of the binding. 
        /// <para>For the keyboard this will be equal to the unity keycode.</para>
        /// <para>For gamepads this will be the name of the input set up in the project settings</para>
        /// </summary>
        public string KeyCode { get; private set; }

        /// <summary>
        /// In unity, all inputs are a value between -1 and 1.
        /// In some cases, We want to bind the -1 and 1 parts of the 
        /// axis separately.  
        /// <para>In these scenarios, we use the axis direction to determine
        /// which direction is considered a key press.</para>
        /// <para>For a positive axis type, the key is considered pressed if CurrentAxis > DeadZone</para>
        /// <para>For a negative axis type, the key is considered pressed if CurrentAxis < DeadZone</para>
        /// <para>Regardless of this value, the GetAxis method always works the same</para>
        /// </summary>
        public AxisDirections AxisDirection { get; private set; }

        /// <summary>
        /// The amount to ignore when calculating key presses 
        /// </summary>
        public float DeadZone { get; private set; }

        /// <summary>
        /// Marks this key input to be read as an axis, allowing it to be checked for KeyHit() and KeyUp() 
        /// </summary>
        public bool ReadAsAxis { get; private set; }

        /// <summary>
        /// Instantiate new KeyBindingValue
        /// </summary>
        /// <param name="keyCode">Corresponds to the key code of the input. See <seealso cref="KeyCode"/> for more info.</param>
        /// <param name="axisDirection"></param>
        /// <param name="readAsAxis"></param>
        /// <param name="deadZone"></param>
        public KeyCodeValue(string keyCode, AxisDirections axisDirection = AxisDirections.Positive, bool readAsAxis = false, float deadZone = 0.19f)
        {
            KeyCode = keyCode;
            AxisDirection = axisDirection;
            ReadAsAxis = readAsAxis;
            DeadZone = deadZone;
        }

    }
}

using Assets.Source.Input.Interfaces;

namespace Assets.Source.Input.Base
{
    public abstract class InputManagerBase
    {
        private IInputListener activeListener;

        public InputManagerBase(IInputListener initialInputListener)
        {
            activeListener = initialInputListener;
        }

        /// <summary>
        /// Returns true when the key is first pressed down, or was not being held down but now is
        /// </summary>
        public bool IsKeyPressed(string binding)
        {
            return activeListener.IsKeyHit(binding);
        }

        /// <summary>
        /// Returns true if the key is released, or was being held down but now is not 
        /// </summary>
        public bool IsKeyReleased(string binding)
        {
            return activeListener.IsKeyReleased(binding);
        }

        /// <summary>
        /// Returns true while the key is being held down
        /// </summary>
        public bool IsKeyHeld(string binding)
        {
            return activeListener.IsKeyHeld(binding);
        }

        /// <summary>
        /// Gets a value between 0 and 1, 1 being fully pressed, 0 being fully not-pressed.
        /// <para>This value is affected by the axis direction of the key binding value</para>
        /// </summary>
        public float GetAxisValue(string binding)
        {
            return activeListener.GetAxis(binding);
        }

        /// <summary>
        /// Returns the active input listener
        /// </summary>
        /// <returns></returns>
        public IInputListener GetActiveListener()
        {
            return activeListener;
        }

        /// <summary>
        /// This method is used to set the active input listener to the one specified.  It must exist in the list 
        /// </summary>
        public void SwapInputModes(IInputListener inputListener)
        {
            activeListener = inputListener;
        }

    }
}

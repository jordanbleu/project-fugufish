namespace Assets.Source.Input.Interfaces
{
    public interface IInputListener
    {
        /// <summary>
        /// Returns a float between -1 and 1 for the input's axis value
        /// <para>For buttons, 1 is fully pressed and 0 is fully released. </para>
        /// <para>For joysticks, -1 is fully one way and 1 is fully the other way, while 0 is fully centered </para>
        /// </summary>
        float GetAxis(string binding);

        bool IsKeyReleased(string binding);

        bool IsKeyHeld(string binding);

        bool IsKeyHit(string binding);


        /// <summary>
        /// Used to load the key bindings either from the config file or from memory.
        /// </summary>
        IBindings GetKeyBindings();

        /// <summary>
        /// Returns true if no physical buttons are pressed, all axes are in a neutral position
        /// </summary>
        bool IsNeutral();

        /// <summary>
        /// This gets called every frame and should update whatever internal input values need to be updated
        /// </summary>
        void UpdateInputList();

    }
}

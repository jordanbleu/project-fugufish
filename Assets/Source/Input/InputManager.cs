using Assets.Source.Input.Base;
using Assets.Source.Input.Interfaces;

namespace Assets.Source.Input
{
    public class InputManager : InputManagerBase
    {
        public InputManager(IInputListener initialInputListener) : base(initialInputListener) { }
    }
}

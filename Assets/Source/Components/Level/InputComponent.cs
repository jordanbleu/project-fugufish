using Assets.Source.Input;

namespace Assets.Source.Components.Level
{
    public class InputComponent : ComponentBase
    {

        public InputManager InputManager { get; private set; }

        public override void ComponentAwake()
        {
            InputManager = new InputManager(new KeyboardInputListener());
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {

            // todo: swap between input modes

            InputManager.GetActiveListener().UpdateInputList();
            base.ComponentUpdate();
        }


    }
}

using Assets.Source.Input;
using Assets.Source.Scene;

namespace Assets.Source.Components.Level
{
    public class InputComponent : ComponentBase
    {
        private InputManager _inputManager;
        public InputManager InputManager 
        {
            get
            {
                if (_inputManager == null)
                {
                    if (GameDataTracker.IsUsingController)
                    {
                        _inputManager = new InputManager(new GamepadInputListener());
                    }
                    else { 
                        _inputManager = new InputManager(new KeyboardInputListener());
                    }
                }
                return _inputManager;
            }
        }

        public override void ComponentUpdate()
        {
            InputManager.GetActiveListener().UpdateInputList();
            base.ComponentUpdate();
        }


    }
}

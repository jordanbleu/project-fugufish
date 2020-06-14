using Assets.Source.Configuration.Base;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using Assets.Source.Input.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Source.Configuration
{
    [Serializable]
    public class GamepadBindings : ConfigurationBase, IBindings
    {
        public Dictionary<string, IEnumerable<KeyCodeValue>> Bindings { get; set; } = new Dictionary<string, IEnumerable<KeyCodeValue>>()
        {
            // Menu Controls
            { InputConstants.K_MENU_UP,         new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_DPAD_V, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_MENU_DOWN,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_DPAD_V, KeyCodeValue.AxisDirections.Negative, true)} },
            { InputConstants.K_MENU_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_DPAD_H, KeyCodeValue.AxisDirections.Negative, true)} },
            { InputConstants.K_MENU_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_DPAD_H, KeyCodeValue.AxisDirections.Positive, true)} },
            { InputConstants.K_MENU_ENTER,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_ABUTTON) } },
            { InputConstants.K_MENU_BACK,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_BBUTTON)} },

            // Player Controls
            { InputConstants.K_MOVE_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Negative, true) } },
            { InputConstants.K_MOVE_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_MOVE_DOWN,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_MOVE_UP,         new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Negative, true) } },
            { InputConstants.K_ATTACK_PRIMARY,  new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_ABUTTON) } },
            { InputConstants.K_PAUSE,           new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_START_BUTTON) } },
            { InputConstants.K_DASH_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTTRIGGER, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_DASH_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_RIGHTTRIGGER, KeyCodeValue.AxisDirections.Positive, true) } },
        };
    }
}

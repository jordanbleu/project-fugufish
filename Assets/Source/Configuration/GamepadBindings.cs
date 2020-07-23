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
            // Player Controls
            { InputConstants.K_MOVE_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Negative, true) } },
            { InputConstants.K_MOVE_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_MOVE_DOWN,       new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_MOVE_UP,         new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Negative, true) } },
            
            { InputConstants.K_PAUSE,           new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_START_BUTTON) } },
            
            { InputConstants.K_DODGE_LEFT,      new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTTRIGGER, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_DODGE_RIGHT,     new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_RIGHTTRIGGER, KeyCodeValue.AxisDirections.Positive, true) } },
            { InputConstants.K_SWING_SWORD,     new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_XBUTTON) } },

            { InputConstants.K_INTERACT,        new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_ABUTTON) } },
            { InputConstants.K_SPRINT,          new List<KeyCodeValue>() { new KeyCodeValue(GamepadConstants.GP_LEFTBUMPER) } },
        };
    }
}

using Assets.Source.Configuration;
using Assets.Source.Configuration.Exception;
using Assets.Source.Input.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Assets.Source.Input
{
    public class GamepadInputListener : IInputListener
    {
        private IBindings bindings;

        // This is the threshold that an axis value needs to be in
        // order for the key to be considered pressed
        private readonly float axisButtonInputThreshold = 0.75f;

        // The list of key bindings that are marked as "readAsAxis"
        //private readonly IEnumerable<KeyValuePair<string, KeyCodeValue>> axisKeyCodes;

        // The key queue
        private Dictionary<string, bool> axisDictionary = new Dictionary<string, bool>();

        public GamepadInputListener()
        {
            var axisBindings = GetKeyBindings().Bindings.Where(b => b.Value.First().ReadAsAxis);

            // populate the key code list
            foreach (KeyValuePair<string, IEnumerable<KeyCodeValue>> kvp in axisBindings)
            {
                axisDictionary.Add(kvp.Key, false);
            }
        }

        public IBindings GetKeyBindings()
        {
            if (bindings == null)
            {
                bindings = ConfigurationRepository.GamepadBindings;
            }
            return bindings;
        }

        /// <summary>
        /// Should be called every frame 
        /// </summary>
        public void UpdateInputList()
        {
            foreach (string axisBinding in axisDictionary.Keys.ToList())
            {
                float axisValue = GetAxis(axisBinding);
                bool isDown = axisValue >= axisButtonInputThreshold;
                axisDictionary[axisBinding] = isDown;
            }
        }

        // todo: add deadzone support (trello card exists)
        public float GetAxis(string binding)
        {
            float realAxis = UnityEngine.Input.GetAxis(GetKeyCodeString(binding));

            KeyCodeValue keyCodeValue = GetKeyCodeValue(binding);

            if (keyCodeValue.AxisDirection == KeyCodeValue.AxisDirections.Positive)
            {
                if (realAxis > keyCodeValue.DeadZone)
                {
                    return realAxis;
                }
            }
            else
            {
                // ironically, we want this to be a positive number
                float negativeAxis = -realAxis;

                if (negativeAxis > keyCodeValue.DeadZone)
                {
                    return negativeAxis;
                }
            }
            return 0f;
        }

        public bool IsKeyHeld(string binding)
        {
            KeyCodeValue keyCodeValue = GetKeyCodeValue(binding)
                ?? throw new InvalidConfigurationException<GamepadBindings>($"Unable to find a keybinding for {binding}");

            // read it as a button 
            bool isDown = UnityEngine.Input.GetButton(keyCodeValue.KeyCode);

            if (!isDown)
            {
                // if that returns false, try as an axis
                isDown = UnityEngine.Input.GetAxis(keyCodeValue.KeyCode) > axisButtonInputThreshold;
            }
            return isDown;
        }

        public bool IsKeyHit(string binding)
        {
            KeyCodeValue keyCodeValue = GetKeyCodeValue(binding)
                ?? throw new InvalidConfigurationException<GamepadBindings>($"Unable to find a keybinding for {binding}");

            if (keyCodeValue.ReadAsAxis)
            {
                // if the button was previously not pressed and is now pressed
                bool wasHeld = axisDictionary[binding];
                if (!wasHeld)
                {
                    return GetAxis(binding) > axisButtonInputThreshold;
                }
                return false;
            }

            return UnityEngine.Input.GetButtonDown(GetKeyCodeString(binding));
        }

        public bool IsKeyReleased(string binding)
        {
            KeyCodeValue keyCodeValue = GetKeyCodeValue(binding)
             ?? throw new InvalidConfigurationException<GamepadBindings>($"Unable to find a keybinding for {binding}");

            if (keyCodeValue.ReadAsAxis)
            {
                // if the button was previously not pressed and is now pressed
                bool wasHeld = axisDictionary[binding];

                if (wasHeld)
                {
                    return GetAxis(binding) <= axisButtonInputThreshold;
                }
                return false;
            }

            return UnityEngine.Input.GetButtonUp(GetKeyCodeString(binding));
        }

        public bool IsNeutral()
        {
            foreach (KeyCodeValue keyCodeValue in GetKeyBindings().Bindings.Values)
            {
                float axisValue = UnityEngine.Input.GetAxis(keyCodeValue.KeyCode);

                if (axisValue > keyCodeValue.DeadZone || axisValue < -keyCodeValue.DeadZone)
                {
                    return false;
                }
            }
            return true;
        }

        private KeyCodeValue GetKeyCodeValue(string binding)
        {
            if (TryGetKeyBinding(binding, out KeyCodeValue keyCodeValue))
            {
                return keyCodeValue;
            }
            throw new InvalidConfigurationException<GamepadBindings>($"Unable to find key binding for {binding}");
        }

        // Pulls the bound value for the gamepad input.  Note - Only one binding will be used for gamepad inputs
        private bool TryGetKeyBinding(string binding, out KeyCodeValue keyCodeValues) {
            if (GetKeyBindings().Bindings.TryGetValue(binding, out IEnumerable<KeyCodeValue> values))
            {
                if (values.Count() > 1) {
                    UnityEngine.Debug.LogWarning($"Warning!  Multiple keys are bound to binding '{binding}'.  The first one will be used" +
                        "and the rest will be ignored.");
                }

                keyCodeValues = values.First();
                return true;
            }
            keyCodeValues = null;
            return false;
        }


        private string GetKeyCodeString(string binding)
        {
            return GetKeyCodeValue(binding).KeyCode;
        }


    }


}

using Assets.Source.Configuration;
using Assets.Source.Configuration.Exception;
using Assets.Source.Extensions;
using Assets.Source.Input.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Input
{
    public class KeyboardInputListener : IInputListener
    {
        private IBindings bindings;

        public IBindings GetKeyBindings()
        {
            if (bindings == null)
            {
                bindings = ConfigurationRepository.KeyboardBindings;
            }
            return bindings;
        }

        /// <summary>
        /// Returns the combined axis values for all input types bound to this input.
        /// <para>
        /// for example, if i have "move up" bound to the left AND right analog sticks for some reason,
        /// the combined inputs from each will be returned, but clamped between 0 and 1.  
        /// This means if I move both analog sticks up, I'll get 1.  If i move one up and one
        /// downn, I'll get 0, etc
        /// </para>
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public float GetAxis(string binding)
        {
            IEnumerable<KeyCode> keyCode = GetKeyCodes(binding);

            return Mathf.Clamp(GetKeyCodes(binding)
                .Select(code => UnityEngine.Input.GetKey(code))
                .Select(value => value.ToFloat())
                .Sum(), 0, 1);
        }

        /// <summary>
        /// Checks if any input with this binding is currently being held down this frame
        /// <para>
        /// </summary>
        public bool IsKeyHeld(string binding)
        {
            IEnumerable<KeyCode> keyCodes = GetKeyCodes(binding);
            return keyCodes.Any(kc => UnityEngine.Input.GetKey(kc).Equals(true));
        }

        /// <summary>
        /// Returns true if any input with this binding is currently pressed but was not pressed last frame
        /// </summary>
        public bool IsKeyHit(string binding)
        {
            IEnumerable<KeyCode> keyCodes = GetKeyCodes(binding);
            return keyCodes.Any(kc => UnityEngine.Input.GetKeyDown(kc).Equals(true));
        }

        /// <summary>
        /// Returns true if any input with this binding is currently released but was pressed last frame
        /// </summary>
        public bool IsKeyReleased(string binding)
        {
            IEnumerable<KeyCode> keyCodes = GetKeyCodes(binding);
            return keyCodes.Any(kc => UnityEngine.Input.GetKeyUp(kc).Equals(true));
        }

        /// <summary>
        /// Returns true if no inputs with this binding are currently being pressed / moved / touched 
        /// </summary>
        public bool IsNeutral()
        {
            foreach (string binding in GetKeyBindings().Bindings.Keys)
            {
                if (GetAxis(binding) > 0f)
                {
                    return false;
                }
            }
            return true;
        }

        // Gets the list of key codes that this input binding is bound to 
        private IEnumerable<KeyCode> GetKeyCodes(string binding)
        {
            if (GetKeyBindings().Bindings.TryGetValue(binding, out IEnumerable<KeyCodeValue> keyCodeValues))
            {
                foreach (KeyCodeValue keyCodeValue in keyCodeValues)
                {
                    if (Enum.TryParse(keyCodeValue.KeyCode, out KeyCode keyCode))
                    {
                        yield return keyCode;
                    }
                    else
                    {
                        throw new InvalidConfigurationException<KeyboardBindings>
                            ($"Key binding value '{keyCodeValue.KeyCode}' is not a valid keyboard KeyCode.");
                    }
                }
            }
            else
            { 
                throw new InvalidConfigurationException<KeyboardBindings>($"Unable to find key bindings for {binding}");
            }
        }

        public void UpdateInputList()
        {
            // Do nothing
        }
    }
}

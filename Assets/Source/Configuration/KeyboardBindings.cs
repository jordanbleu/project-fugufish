using Assets.Source.Configuration.Base;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using Assets.Source.Input.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Configuration
{
    [Serializable]
    public class KeyboardBindings : ConfigurationBase, IBindings
    {
        public Dictionary<string, IEnumerable<KeyCodeValue>> Bindings { get; set; } = new Dictionary<string, IEnumerable<KeyCodeValue>>()
        {
            // Player Controls
            { InputConstants.K_MOVE_LEFT,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.A))      } },
            { InputConstants.K_MOVE_RIGHT,     new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.D))      } },
            { InputConstants.K_MOVE_DOWN,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.S))      } },
            { InputConstants.K_MOVE_UP,        new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.W))      } },
            
            { InputConstants.K_PAUSE,          new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Escape)) } },
            
            { InputConstants.K_DODGE_LEFT,     new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Q))      } },
            { InputConstants.K_DODGE_RIGHT,    new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.E))      } },
            { InputConstants.K_SWING_SWORD,    new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Mouse0))  } },
            { InputConstants.K_JUMP,           new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Space))  } },
            { InputConstants.K_SHOWHUD,        new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.F))  } },

            { InputConstants.K_MENU_UP,        new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.UpArrow))  } },
            { InputConstants.K_MENU_DOWN,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.DownArrow))  } },
            { InputConstants.K_MENU_ENTER,     new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Return))  } },
        };

        private static string Stringify(KeyCode key)
        {
            return ((int)key).ToString();
        }
    }
}

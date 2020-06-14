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
            
            { InputConstants.K_SHOOT_LEFT,     new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.J))      } },
            { InputConstants.K_SHOOT_RIGHT,    new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.L))      } },
            { InputConstants.K_SHOOT_DOWN,     new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.K))      } },
            { InputConstants.K_SHOOT_UP,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.I))      } },
            
            { InputConstants.K_HIDE,           new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Space))  } }
        };

        private static string Stringify(KeyCode key)
        {
            return ((int)key).ToString();
        }
    }
}

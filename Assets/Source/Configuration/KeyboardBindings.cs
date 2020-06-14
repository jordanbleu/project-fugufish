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
            // Menu Controls
            { InputConstants.K_MENU_UP,         new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.UpArrow))    } },
            { InputConstants.K_MENU_DOWN,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.DownArrow))  } },
            { InputConstants.K_MENU_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.LeftArrow))  } },
            { InputConstants.K_MENU_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.RightArrow)) } },
            { InputConstants.K_MENU_ENTER,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Return))     } },
            { InputConstants.K_MENU_BACK,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Escape))     } },

            // Player Controls
            { InputConstants.K_MOVE_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.A))      } },
            { InputConstants.K_MOVE_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.D))      } },
            { InputConstants.K_MOVE_DOWN,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.S))      } },
            { InputConstants.K_MOVE_UP,         new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.W))      } },
            
            { InputConstants.K_ATTACK_PRIMARY,  new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Space)),
                                                                           new KeyCodeValue(Stringify(KeyCode.Mouse0)) } },

            { InputConstants.K_PAUSE,           new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Escape)) } },
            { InputConstants.K_DASH_LEFT,       new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.Q))      } },
            { InputConstants.K_DASH_RIGHT,      new List<KeyCodeValue>() { new KeyCodeValue(Stringify(KeyCode.E))      } }
        };

        private static string Stringify(KeyCode key)
        {
            return ((int)key).ToString();
        }
    }
}

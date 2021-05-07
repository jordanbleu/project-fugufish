using UnityEngine;

namespace Assets.Source.Components.Switches.Base
{
    public class SwitchComponentBase : ComponentBase
    {
        [SerializeField]
        protected bool isOn;

        /// <summary>
        /// True if the switch is on / enabled or whatever
        /// </summary>
        public bool IsOn { get => isOn; }
    }
}

using Assets.Source.Components.Switches.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Objects
{
    /// <summary>
    /// When all switches under this parent object are enabled, will open the door.  Otherwise the door is closed.
    /// </summary>
    public class DoorMultiSwitchComponent : ComponentBase
    {

        [SerializeField]
        private DoorComponent door;

        [SerializeField]
        private List<SwitchComponentBase> switches;

        public override void ComponentUpdate()
        {
            // All switches are on
            if (!switches.Any(s => !s.IsOn)) {
                if (!door.IsOpen) {
                    door.Toggle();
                }
            }
            else {
                if (door.IsOpen) {
                    door.Toggle();
                }
            }

            base.ComponentUpdate();
        }

    }
}

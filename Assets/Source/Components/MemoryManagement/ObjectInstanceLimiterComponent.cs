using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.MemoryManagement
{
    /// <summary>
    /// Tracks instances of a given prefab within the scene, and automatically destroys the oldest instance when the limit is reached.
    /// This probably isn't the best way to do this but hey it works™
    /// Pro Tip - GameObjectEmitter can hook into this if you want to use that
    /// </summary>
    public class ObjectInstanceLimiterComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("Tracks a given object and manages instances based on the provided settings")]
        private List<LimitSettings> limitOptions;

        private Dictionary<string, LimitSettings> limits;
        private Dictionary<string, List<GameObject>> instances;

        public override void ComponentPreStart()
        {
            // Convert to a dictionary to faster lookups, thank you to unity for not supporting this >:(
            limits = limitOptions.ToDictionary(l => l.Id, l => l);
            instances = limitOptions.ToDictionary(l => l.Id, l=>new List<GameObject>());
            base.ComponentPreStart();
        }

        /// <summary>
        /// Call this from an external object to add to the object pool
        /// </summary>
        /// <param name="id">The ID of the object instance that you specified in the inspector</param>
        /// <param name="instance">The instance to keep track of</param>
        public void TrackNewInstance(string id, GameObject instance) {
            
            if (!limits.ContainsKey(id)) {
                throw new UnityException($"Key of '{id}' does not exist in the referenced instance limiter '{gameObject.name}'.  " +
                    $"Keys must be defined via the inspector before using that.");
            }

            instances[id].Add(instance);

            if (instances[id].Count() > limits[id].Amount) {
                // Destroy the first object in the list
                Destroy(instances[id].First());
            }

        }

        [Serializable]
        public struct LimitSettings {
            [Tooltip("An arbitrary ID for this instance.")]
            public string Id;
            [Tooltip("How many instances of this object are allowed before they begin getting destroyed?")]
            public int Amount;
        }
    }
}

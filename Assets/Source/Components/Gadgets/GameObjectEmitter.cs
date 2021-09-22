using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Source.Components.Gadgets
{
    public class GameObjectEmitter : ComponentBase
    {

        [Tooltip("The range for the object to spawn within (randomized each time)")]
        [SerializeField]
        private float EmissionRangeWidth = 2f;

        [Tooltip("The range for the object to spawn within (randomized each time)")]
        [SerializeField]
        private float EmissionRangeHeight = 2f;

        // This will be converted to a dictionary where the key is the prefab's name and the value is the list of instances of that prefab
        [SerializeField]
        [Tooltip("Used to limit instances of a particular prefab emitted by this emitter.  Once the count of instances of a prefab is reached," +
            "the first / oldest instance will be destroyed automatically, if it hasn't been destroyed already.")]
        private List<EmissionLimit> emissionLimits;

        [SerializeField]
        [TextArea(0,200)]
        private string currentLimits;

        private Dictionary<string, EmissionLimit> limits;
        private Dictionary<string, List<GameObject>> instances;

        public override void ComponentPreStart()
        {
            // Check for duplicate names
            if (emissionLimits.Any(el=>emissionLimits.Count(otherEl => otherEl.GameObjectName.Equals(el.GameObjectName)) > 1)) {
                var badName = emissionLimits.First(el => emissionLimits.Count(otherEl => otherEl.GameObjectName.Equals(el.GameObjectName)) > 1);
                throw new UnityException($"Found multiple EmissionLimits for the object of '{badName}'.  Please do not do that.");
            }

            // Convert to a dictionary to faster lookups, thank you to unity for not supporting this >:(
            limits = emissionLimits.ToDictionary(l => l.GameObjectName, l => l);
            instances = emissionLimits.ToDictionary(l => l.GameObjectName, l => new List<GameObject>());
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {

            // Display debug text
            if (Application.isEditor) {
                var sb = new StringBuilder();

                foreach (var kvp in instances) {
                    sb.Append(kvp.Key);
                    sb.Append($" : {kvp.Value.Count()}/{limits[kvp.Key].Limit}");
                    sb.AppendLine();
                }

                currentLimits = sb.ToString();
            }
            base.ComponentUpdate();
        }

        public void Emit(GameObject obj) {
            var xPos = UnityEngine.Random.Range(transform.position.x - (EmissionRangeWidth / 2), transform.position.x + (EmissionRangeWidth / 2));
            var yPos = UnityEngine.Random.Range(transform.position.y - (EmissionRangeHeight / 2), transform.position.y + (EmissionRangeHeight / 2));

            var inst = Instantiate(obj, transform.parent);
            inst.transform.position = new Vector3(xPos, yPos, transform.position.z);

            // Update our limits if necessary
            if (limits.ContainsKey(obj.name)) {

                // Add to our tracked instance dictionary
                instances[obj.name].Add(inst);
                
                var limit = limits[obj.name].Limit;

                if (instances[obj.name].Count() > limit) {

                    if (UnityUtils.Exists(instances[obj.name].First()))
                    {
                        // destroy the first (oldest) instance in the list if it isn't already destroyed.
                        Destroy(instances[obj.name].First());
                    }
                    // remove from the list, shifting all other elements 
                    instances[obj.name].Remove(instances[obj.name].First());
                }
            }

        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityUtils.Color(255, 128, 75);
            Gizmos.DrawWireCube(transform.position, new Vector3(EmissionRangeWidth / 2, EmissionRangeHeight / 2, 0));
        }

        [Serializable]
        public struct EmissionLimit {
            [Tooltip("The name of the prefab game object to track")]
            public string GameObjectName;
            [Tooltip("The number to limit instances of")]
            public int Limit;
        }

    }
}

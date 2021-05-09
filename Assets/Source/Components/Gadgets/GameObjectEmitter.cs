using Assets.Source.Math;
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


        public void Emit(GameObject obj) {
            var xPos = Random.Range(transform.position.x - (EmissionRangeWidth / 2), transform.position.x + (EmissionRangeWidth / 2));
            var yPos = Random.Range(transform.position.y - (EmissionRangeHeight / 2), transform.position.y + (EmissionRangeHeight / 2));

            var inst = Instantiate(obj, transform.parent);
            inst.transform.position = new Vector3(xPos, yPos, transform.position.z);

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityUtils.Color(255, 128, 75);
            Gizmos.DrawWireCube(transform.position, new Vector3(EmissionRangeWidth / 2, EmissionRangeHeight / 2, 0));
        }

    }
}

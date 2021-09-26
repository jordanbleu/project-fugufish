using Assets.Source.Math;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.MemoryManagement
{
    public class GameBoundsComponent : ComponentBase
    {
        [Tooltip("The range for the object to remain active")]
        [SerializeField]
        private float RangeWidth = 2f;

        [Tooltip("The range for the object to remain active")]
        [SerializeField]
        private float RangeHeight = 2f;


        [SerializeField]
        [Tooltip("Event to trigger when the object goes out of bounds")]
        private UnityEvent onOutOfBounds = new UnityEvent();

        // These values are baked in when the game runs
        private float setRangeWidthMin = 0f;
        private float setRangeWidthMax= 0f;
        private float setRangeHeightMin = 0f;
        private float setRangeHeightMax = 0f;
        private Vector2 setPosition = Vector2.zero;

        public override void ComponentStart()
        {
            setPosition = transform.position;
            setRangeWidthMin = transform.position.x - (RangeWidth / 2);
            setRangeWidthMax = transform.position.x + (RangeWidth / 2);
            setRangeHeightMin = transform.position.y - (RangeHeight / 2);
            setRangeHeightMax = transform.position.y + (RangeHeight / 2);

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (transform.position.x < setRangeWidthMin ||
                transform.position.x > setRangeWidthMax ||
                transform.position.y < setRangeHeightMin ||
                transform.position.y > setRangeHeightMax) {

                onOutOfBounds?.Invoke();
            }
            base.ComponentUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityUtils.Color(200, 130, 30);
            if (setRangeWidthMin < 0f || setRangeWidthMin > 0f)
            {
                Gizmos.DrawWireCube(setPosition, new Vector3(RangeWidth / 2, RangeHeight / 2, 0));
            }
            else {
                Gizmos.DrawWireCube(transform.position, new Vector3(RangeWidth / 2, RangeHeight / 2, 0));
            }
        }

    }
}

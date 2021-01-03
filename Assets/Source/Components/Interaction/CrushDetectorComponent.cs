using Assets.Source.Math;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// Performs raycasts to see if the item is being crushed
    /// </summary>
    public class CrushDetectorComponent : ComponentBase
    {
        [SerializeField]
        private float verticalSize = 0.75f;

        [SerializeField]
        private float horizontalSize = 0.25f;

        [SerializeField]
        private UnityEvent onCrush;

        [SerializeField]
        [Tooltip("The actor can be crushed by anything included in this layer mask")]
        private LayerMask layersThatDoTheCrush;

        public override void ComponentUpdate()
        {
            var upCast = Physics2D.RaycastAll(transform.position, Vector2.up, verticalSize, layersThatDoTheCrush);
            var downCast = Physics2D.RaycastAll(transform.position, Vector2.down, verticalSize, layersThatDoTheCrush);

            // We're being crushed 
            if (AnySolids(upCast) && AnySolids(downCast))
            {
                onCrush?.Invoke();
            }
            else { 
                var leftCast = Physics2D.RaycastAll(transform.position, Vector2.left, horizontalSize, layersThatDoTheCrush);
                var rightCast = Physics2D.RaycastAll(transform.position, Vector2.right, horizontalSize, layersThatDoTheCrush);

                if (AnySolids(leftCast) && AnySolids(rightCast)) {
                    onCrush?.Invoke();                
                }
            }
            base.ComponentFixedUpdate();
        }

        private bool AnySolids(RaycastHit2D[] raycasts) {
            // return true if any raycast is colliding with an object that is:
            // * has a collider
            // * collider is not a trigger
            // * not my object
            foreach (var hit in raycasts) {
                if (hit.transform != null && 
                    hit.transform.gameObject.TryGetComponent<Collider2D>(out var collider) &&
                    !collider.isTrigger &&
                    hit.transform.gameObject != gameObject) 
                {
                    return true;                                    
                }
            }
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            // up
            Gizmos.color = UnityUtils.Color(255, 0, 0);
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + verticalSize));

            // down
            Gizmos.color = UnityUtils.Color(0, 0, 255);
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - verticalSize));

            // left
            Gizmos.color = UnityUtils.Color(0, 255, 0);
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - horizontalSize, transform.position.y));
            
            // right
            Gizmos.color = UnityUtils.Color(255, 255, 255);
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + horizontalSize, transform.position.y));

        }

    }
}

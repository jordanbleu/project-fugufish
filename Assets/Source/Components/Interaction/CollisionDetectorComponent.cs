using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// Wrapper component for automatically invoking events on trigger
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDetectorComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("If true, the unity event will fired repeatedly on each frame that the object is colliding with something.")]
        private bool isContinuous;

        [SerializeField]
        private UnityEvent onExit;

        [SerializeField]
        private UnityEvent onEnter;

        [SerializeField]
        private LayerMask colliderLayers;

        // Track whether trigger was triggered last frame
        private bool wasTriggered = false;
               

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isContinuous || !wasTriggered)
            {
                if (colliderLayers.IncludesLayer(collision.gameObject.layer))
                {
                    onEnter.Invoke();
                    wasTriggered = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (colliderLayers.IncludesLayer(collision.gameObject.layer))
            {
                onExit.Invoke();
                wasTriggered = false;
            }            
        }

    }
}

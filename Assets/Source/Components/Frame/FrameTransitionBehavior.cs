using Assets.Source.Components.Player;
using Cinemachine;
using System;
using UnityEngine;

namespace Assets.Source.Components.Frame
{
    /// <summary>
    /// Handles the transition to a new frame
    /// </summary>
    public class FrameTransitionBehavior : ComponentBase
    {
        /// <summary>
        /// Frame we are transitioning from
        /// </summary>
        public GameObject SourceFrame { get; set; }

        /// <summary>
        /// Frame we are transitioning to
        /// </summary>
        public GameObject DestinationFrame { get; set; }

        /// <summary>
        /// The position in world space our player will start in the <seealso cref="DestinationFrame"/>
        /// </summary>
        public Vector3 StartPosition { get; set; }

        // used to track the virtual cameras follow object
        private GameObject followObject;

        public override void ComponentStart()
        {
            if (SourceFrame == null || DestinationFrame == null) {
                throw new InvalidOperationException("SourceFrame and DestinationFrame must not be null");
            }

            var sourceVCam = GetRequiredComponentInChildren<CinemachineVirtualCameraBase>(SourceFrame) 
                ?? throw new InvalidOperationException($"SourceFrame '{SourceFrame}' must have a Virtual Camera as a child object");
            var destinationVCam  = GetRequiredComponentInChildren<CinemachineVirtualCameraBase>(DestinationFrame)
                ?? throw new InvalidOperationException($"DestinationFrame '{DestinationFrame}' must have a Virtual Camera as a child object");

            if (sourceVCam.Follow != destinationVCam.Follow) {
                throw new InvalidOperationException($"SourceFrame virtual camera '{SourceFrame}/{sourceVCam}' must be following the same object " +
                    $"as the destination frame camera '{DestinationFrame}/{destinationVCam}'");
            }

            followObject = sourceVCam.Follow.gameObject;

            // disable player physics while the transition is in progress
            followObject.SetActive(false);

            base.ComponentStart();
        }

        /// <summary>
        /// Called from the timeline animation when the fade out is finished
        /// </summary>
        public void OnFadeOutAnimationCompleted()
        {
            // Disable the frame we are coming from, freeing up CPU / GPU resources 
            SourceFrame.SetActive(false);

            // Teleport the player to the new position, maintain it's Z position though
            followObject.transform.position = new Vector3(StartPosition.x, StartPosition.y, followObject.transform.position.z);

            // Disable player physics while the transition is in progress
            var playerComponent = GetRequiredComponent<PlayerPhysicsComponent>(followObject);

            // reenable player physics while the transition is in progress
            followObject.SetActive(true);

            // Enable the frame we are transitioning to
            DestinationFrame.SetActive(true);
        
        }

        /// <summary>
        /// Called from the timeline animation when the fade in is finished
        /// </summary>
        public void OnFadeInAnimationCompleted() {
            // Kill self
            Destroy(gameObject);
            

        }

    }
}

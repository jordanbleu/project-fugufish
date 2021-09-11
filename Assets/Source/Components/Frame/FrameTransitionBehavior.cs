using Assets.Source.Components.Level;
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

        private LevelComponent levelComponent;

        // used to track the virtual cameras follow object
        private GameObject followObject;

        public override void ComponentAwake()
        {
            levelComponent = GetRequiredComponent<LevelComponent>(GetRequiredObject("Level"));
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {

            if (!UnityUtils.Exists(SourceFrame) || !UnityUtils.Exists(DestinationFrame)) {
                throw new InvalidOperationException("SourceFrame and DestinationFrame must not be null");
            }

            var sourceVCam = GetRequiredComponentInChildren<CinemachineVirtualCameraBase>(SourceFrame);
            var destinationVCam = GetRequiredComponentInChildren<CinemachineVirtualCameraBase>(DestinationFrame);

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
            // Run the source Frames exit event
            GetRequiredComponent<FrameComponent>(SourceFrame).TriggerExitEvent();

            // Disable the frame we are coming from, freeing up CPU / GPU resources 
            SourceFrame.SetActive(false);

            // Teleport the player to the new position, maintain it's Z position though
            followObject.transform.position = new Vector3(StartPosition.x, StartPosition.y, followObject.transform.position.z);

            // reenable player physics while the transition is in progress
            followObject.SetActive(true);

            // Enable the frame we are transitioning to
            DestinationFrame.SetActive(true);

            // Tell the level component what frame we're now on
            levelComponent.CurrentlyActiveFrame = DestinationFrame;

            // Run the new frames enter event
            GetRequiredComponent<FrameComponent>(DestinationFrame).TriggerEnterEvent();

            // Refresh the death marker
            levelComponent.RefreshDeathMarker();
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

using Assets.Source.Components.Frame.Base;
using Assets.Source.Components.Player;
using System;
using UnityEngine;

namespace Assets.Source.Components.Frame
{
    [RequireComponent(typeof(Collider2D))]
    public class FrameTransitionColliderComponent : FramePortalComponentBase
    {

        public override void ComponentAwake()
        {
            if (Debug.isDebugBuild)
            {
                // Validate that we have everything we need
                var boxCollider = GetRequiredComponent<BoxCollider2D>();

                if (!boxCollider.isTrigger)
                {
                    throw new InvalidOperationException($"For CameraBoxColliderPortal '{gameObject.name}', " +
                        $"there's an attached box collider 2d, which is good, but you need to check the 'IsTrigger' box");
                }
            }
            base.ComponentAwake();
        }

        // Occurs when the player enters the collider area
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only React to the player
            if (collision.gameObject.TryGetComponent<PlayerPhysicsComponent>(out _)) {
                
                var frameTransitionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/System/FrameTransition");

                var canvas = FindOrCreateCanvas();

                // Creates the frame transitioner as a child of the canvas object
                var frameTransition = Instantiate(frameTransitionPrefab, canvas.transform);
                var frameTransitionComponent = GetRequiredComponent<FrameTransitionBehavior>(frameTransition);
                frameTransitionComponent.SourceFrame = SourceFrame;
                frameTransitionComponent.DestinationFrame = DestinationFrame;
                frameTransitionComponent.StartPosition = StartPosition;
            }            
        }

    }
}

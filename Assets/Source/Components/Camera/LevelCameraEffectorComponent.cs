using Assets.Source.Components.Level;
using System;
using UnityEngine;

namespace Assets.Source.Components.Camera
{
    /// <summary>
    /// This sits on the level component and is responsible for delegating camera effect requests 
    /// to the active camera
    /// </summary>
    public class LevelCameraEffectorComponent : ComponentBase
    {
        private LevelComponent levelComponent;

        public override void ComponentPreStart()
        {
            levelComponent = GetRequiredComponent<LevelComponent>(GetRequiredObject("Level"));            
            base.ComponentPreStart();
        }



        private Animator GetActiveCameraAnimator() {
            var obj = levelComponent.CurrentlyActiveFrame;

            if (!UnityUtils.Exists(obj))
            {
                Debug.LogWarning($"Unable to find the currently active level frame");
                return null;
            }

            var activeCamera = GetRequiredChild("Camera", obj);

            if (!UnityUtils.Exists(activeCamera)) {
                Debug.LogWarning($"Found active frame '{obj.name}' but couldn't find a Camera object named 'Camera'.");
                return null;
            }

            var activeCameraAnimator = activeCamera.GetComponent<Animator>();

            // may or may not exist!
            return activeCameraAnimator;
        }

        internal void SwingRight()
        {
            var activeCam = GetActiveCameraAnimator();

            if (UnityUtils.Exists(activeCam))
            {
                activeCam.SetTrigger("SwingRight");
            }
        }

        internal void Impact()
        {
            var activeCam = GetActiveCameraAnimator();

            if (UnityUtils.Exists(activeCam))
            {
                activeCam.SetTrigger("Impact");
            }
        }

        internal void LargeImpact()
        {
            var activeCam = GetActiveCameraAnimator();

            if (UnityUtils.Exists(activeCam))
            {
                activeCam.SetTrigger("LargeImpact");
            }
        }
    }
}

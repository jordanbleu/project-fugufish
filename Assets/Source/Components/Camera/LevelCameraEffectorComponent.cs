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

        public override void ComponentAwake()
        {
            levelComponent = GetRequiredComponent<LevelComponent>(GetRequiredObject("Level"));            
            base.ComponentAwake();
        }

        internal void Impact()
        {

            
        }

        private Animator GetActiveCameraAnimator() {
            var obj = levelComponent.CurrentlyActiveFrame;

            if (!UnityUtils.Exists(obj))
            {
                return null;
            }

            return obj.GetComponent<Animator>();
        }
    }
}

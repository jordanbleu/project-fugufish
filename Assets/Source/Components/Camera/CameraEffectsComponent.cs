using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Camera
{
    /// <summary>
    /// Add this to an object to do the camera effects 
    /// </summary>
    public class CameraEffectsComponent : ComponentBase
    {
        private LevelCameraEffectorComponent cameraEffector;

        public override void ComponentPreStart()
        {
            cameraEffector = GetRequiredComponent<LevelCameraEffectorComponent>(GetRequiredObject("Level"));

            if (!UnityUtils.Exists(cameraEffector)) {
                throw new UnityException($"Game Object '{gameObject.name}' can't find a Level object anywhere.");
            }

            base.ComponentPreStart();
        }

        public void Impact() => cameraEffector.Impact();

        public void LargeImpact() => cameraEffector.LargeImpact();


    }
}

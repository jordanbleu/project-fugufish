using Assets.Source.Components.Frame;
using System;
using UnityEngine;

namespace Assets.Source.Components.Level
{

    public class LevelComponent : ComponentBase
    {
        [SerializeField]
        private GameObject activeStartingFrame;

        public override void ComponentAwake()
        {
            SetActiveFrame();
            base.ComponentAwake();
        }

        private void SetActiveFrame()
        {
            if (activeStartingFrame == null) {
                throw new InvalidOperationException("Please set an active frame on the LevelComponent");
            }
                 
            var allFrames = GetComponentsInChildren<FrameComponent>();

            foreach (var frame in allFrames) {
                if (frame.gameObject != activeStartingFrame)
                {
                    frame.gameObject.SetActive(false);
                }
                else {
                    frame.gameObject.SetActive(true);
                }
            }    
        }
    }
}

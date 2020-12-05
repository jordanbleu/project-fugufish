using Assets.Editor.Attributes;
using Assets.Source.Components.Frame;
using System;
using UnityEngine;

namespace Assets.Source.Components.Level
{

    public class LevelComponent : ComponentBase
    {
        [SerializeField]
        private GameObject startingFrame;

        [SerializeField]
        [ReadOnly]
        private GameObject _currentlyActiveFrame;
        public GameObject CurrentlyActiveFrame { get => _currentlyActiveFrame; set => _currentlyActiveFrame = value; }

        public override void ComponentAwake()
        {
            SetActiveFrame();
            base.ComponentAwake();
        }

        private void SetActiveFrame()
        {
            if (startingFrame == null) {
                throw new InvalidOperationException("Please set an active frame on the LevelComponent");
            }

            _currentlyActiveFrame = startingFrame;
     
            var allFrames = GetComponentsInChildren<FrameComponent>();

            foreach (var frame in allFrames) {
                if (frame.gameObject != startingFrame)
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

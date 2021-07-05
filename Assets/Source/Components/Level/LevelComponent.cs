using Assets.Editor.Attributes;
using Assets.Source.Components.Frame;
using Assets.Source.Scene;
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
            
            if (UnityUtils.Exists(GameDataTracker.FrameToLoadOnSceneLoad))
            {
                SetActiveFrame(GameDataTracker.FrameToLoadOnSceneLoad);
            }
            else { 
                SetActiveFrame(startingFrame);
            }

            base.ComponentAwake();
        }

        private void SetActiveFrame(GameObject frameToSet)
        {
            if (frameToSet == null)
            {
                throw new InvalidOperationException("Please set an active frame on the LevelComponent");
            }

            _currentlyActiveFrame = frameToSet;

            var allFrames = GetComponentsInChildren<FrameComponent>();

            // Deactivates all non active frames (just in case I accidentally leave one active in the editor)
            foreach (var frame in allFrames)
            {
                if (frame.gameObject != frameToSet)
                {
                    frame.gameObject.SetActive(false);
                }
                else
                {
                    frame.gameObject.SetActive(true);
                }
            }
            
            
        }
        
    }
}

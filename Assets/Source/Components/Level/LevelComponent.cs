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


        [SerializeField]
        private GameObject deathMarkerPrefab;


        [SerializeField]
        private GameObject player;

        private GameObject deathMarker;

        public override void ComponentAwake()
        {
            player.transform.position = GetRequiredComponent<FrameComponent>(startingFrame).StartPosition;
            
            if (!string.IsNullOrEmpty(GameDataTracker.FrameToLoadOnSceneLoad))
            {
                SetActiveFrame(GetRequiredChild(GameDataTracker.FrameToLoadOnSceneLoad));
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

            var allFrames = GetComponentsInChildren<FrameComponent>(true);

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

            RefreshDeathMarker();
        }

        public void RefreshDeathMarker() { 
        
            if (CurrentlyActiveFrame.name == GameDataTracker.LastDeathFrameName)
            {
                // Create the death marker if we're on that frame
                if (!UnityUtils.Exists(deathMarker))
                {
                    deathMarker = Instantiate(deathMarkerPrefab);
                }
            }
            else {
                // Destroy it if we're not on that frame
                if (UnityUtils.Exists(deathMarker)) {
                    Destroy(deathMarker);
                }
            }
        }
        
    }
}

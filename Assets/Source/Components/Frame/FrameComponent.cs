using UnityEngine;

namespace Assets.Source.Components.Frame
{
    /// <summary>
    /// Frames are small loaded chunks of the level.  This prevents overuse of system resources by updating entire massive levels, 
    /// as well as reduces required load times from loading new scenes.
    /// </summary>
    public class FrameComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("The starting position for the frame")]
        private Vector2 startPosition;
        public Vector2 StartPosition { get => startPosition; }
    }
}

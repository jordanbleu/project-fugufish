using UnityEngine;

namespace Assets.Source.Components.Frame.Base
{
    public abstract class FramePortalComponentBase : ComponentBase
    {

        [SerializeField]
        [Tooltip("The frame we are transitioning from")]
        protected GameObject SourceFrame;

        [SerializeField]
        [Tooltip("The frame we are transitioning to")]
        protected GameObject DestinationFrame;

        [SerializeField]
        [Tooltip("The starting position for the player within the destination frame")]
        protected Vector3 StartPosition;

    }
}

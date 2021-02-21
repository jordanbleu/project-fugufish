using UnityEngine;

namespace Assets.Source.Components.Parallax
{
    /// <summary>
    /// Used for static backgrounds that are very far away, and thus actually just stick to the camera
    /// </summary>
    public class LockToCamera2DComponent : ComponentBase
    {
        [SerializeField]
        [Tooltip("Drag your main camera here.  If using Cinemachine, drag your virtual camera instead.")]
        private Transform followCamera;

        private float xOffset;
        private float yOffset;

        public override void ComponentAwake()
        {

            if (!UnityUtils.Exists(followCamera)) {
                throw new UnityException($"Please drag your main camera into the LockToCameraComponent on object '{gameObject.name}'");
            }
            xOffset = transform.position.x - followCamera.transform.position.x;
            yOffset = transform.position.y - followCamera.transform.position.y;
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            transform.position = new Vector3(followCamera.position.x + xOffset, followCamera.position.y + yOffset, transform.position.z);
            base.ComponentFixedUpdate();
        }

    }
}

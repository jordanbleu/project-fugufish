using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.UI
{
    [RequireComponent(typeof(Animator))]
    public class DynamicMenuItemComponent : ComponentBase
    {

        [SerializeField]
        private GameObject textMeshObject;

        private TextMeshProUGUI textMesh;
        private Animator animator;

        public bool IsHighlighted { get; set; } = false;

        public override void ComponentAwake()
        {
            textMesh = GetRequiredComponent<TextMeshProUGUI>(textMeshObject);
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            animator.SetBool("is_highlighted", IsHighlighted);
            base.ComponentUpdate();
        }

        public void TriggerSelectAnimation()
        {
            animator.SetTrigger("selected");
        }

        public void Setup(string text) {
            textMesh.SetText(text);
        }
    }
}

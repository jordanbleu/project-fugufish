using TMPro;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    /// <summary>
    /// Displays an info message while the user is colliding with a trigger, then hides it when the user is not.
    /// 
    /// Requires a InfoMessageTriggerComponent to send this message
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class InfoMessageComponent : ComponentBase
    {
        private Animator animator;
        private TextMeshProUGUI textMesh;
        private string messageToSet = string.Empty;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            textMesh = GetRequiredComponent<TextMeshProUGUI>();
            base.ComponentAwake();
        }

        public void ShowMessage(string message) {
            messageToSet = message;
            animator.SetTrigger("fade_in");
        }


        /// <summary>
        /// Called from animation timeline when the text has begun fading in (but isn't visible)
        /// </summary>
        public void OnBeginFadeIn() {
            textMesh.SetText(messageToSet);
        }

    }
}

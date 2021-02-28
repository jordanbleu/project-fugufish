using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.UI
{
    public class FadeListenerComponent : ComponentBase
    {
        [SerializeField]
        private UnityEvent onFadeIn = new UnityEvent();

        [SerializeField]
        private UnityEvent onFadeOut = new UnityEvent();

        public void OnFadeIn() => onFadeIn?.Invoke();

        public void OnFadeOut() => onFadeOut?.Invoke();

    }
}

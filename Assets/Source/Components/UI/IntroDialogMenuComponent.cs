using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class IntroDialogMenuComponent : ComponentBase
    {

        [SerializeField]
        private GameObject eventSystem;

        [SerializeField]
        private GameObject firstButton;

        [SerializeField]
        private UnityEvent onSelected = new UnityEvent();

        private Animator animator;

        private EventSystem eventSystemComponent;

        public int SelectedValue { get; set; } = -1;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();            
            base.ComponentAwake();
        }

        public void OnMenuOpen() 
        {
            eventSystemComponent.firstSelectedGameObject = firstButton;                                 
        }

        public void OnMenuClose() 
        {
            gameObject.SetActive(false);
        }

        public void SelectValue(int value) 
        {
            SelectedValue = value;
            onSelected?.Invoke();
            animator.SetTrigger("close");
        }





    }
}

using Assets.Editor.Attributes;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.UI
{
    /// <summary>
    /// The unity UI system is so bad i had to make my own :(
    /// </summary>
    public class DynamicMenuComponent : ComponentBase
    {
        [SerializeField]
        private GameObject menuItemPrefab;

        [SerializeField]
        private MenuItem[] menuItems;

        [SerializeField]
        private float itemHeight = 20;

        private Animator animator;

        // Cache an ordered array of menu item component references
        private DynamicMenuItemComponent[] itemComponents;

        [SerializeField]
        [ReadOnly]
        int highlightedIndex = 0;

        [SerializeField]
        [ReadOnly]
        int selectedIndex = -1;


        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            itemComponents = new DynamicMenuItemComponent[menuItems.Length];

            for (var i = 0; i < menuItems.Length;i++) {
                var inst = Instantiate(menuItemPrefab, transform);
                inst.transform.localPosition = new Vector2(0, (itemHeight - (itemHeight * (i+1))));
                inst.name = $"'{menuItems[i].Text}' - Menu Item";
                itemComponents[i] = GetRequiredComponent<DynamicMenuItemComponent>(inst);
                itemComponents[i].Setup(menuItems[i].Text);
                // menu items start out inactive
                itemComponents[i].gameObject.SetActive(false);
            }

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (Input.IsKeyPressed(InputConstants.K_MENU_UP)) {
                itemComponents[highlightedIndex].IsHighlighted = false;
                highlightedIndex--;
            }
            else if (Input.IsKeyPressed(InputConstants.K_MENU_DOWN)) {
                itemComponents[highlightedIndex].IsHighlighted = false;
                highlightedIndex++;
            }

            // Keep selected index in bounds
            if (highlightedIndex < 0)
            {
                highlightedIndex = menuItems.Length-1;
            }
            else if (highlightedIndex > menuItems.Length-1) {
                highlightedIndex = 0;    
            }

            if (Input.IsKeyPressed(InputConstants.K_MENU_ENTER)) {

                selectedIndex = highlightedIndex;

                // Dectivate all menu items 
                foreach (var comp in itemComponents)
                {
                    comp.gameObject.SetActive(false);
                }

                animator.SetTrigger("close");
            }

            itemComponents[highlightedIndex].IsHighlighted = true;
            base.ComponentUpdate();
        }

        #region Animation Events
        public void OnMenuOpen() {
            // Activate all menu items
            foreach (var comp in itemComponents) {
                comp.gameObject.SetActive(true);
            }

            // Highlight the first item
            itemComponents[0].IsHighlighted = true;
        }

        public void OnMenuClose() {
            // Trigger the event of the selected menu item
            menuItems[selectedIndex].OnItemSelected?.Invoke();            
        }

        #endregion


        [Serializable]
        public struct MenuItem {
            public string Text;
            public UnityEvent OnItemSelected;
        }

    }
}

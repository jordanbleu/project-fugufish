using Assets.Source.Scene;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.Components.UI
{
    public class TitleScreenComponent : ComponentBase
    {

        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (UnityEngine.Input.anyKey) {
                animator.SetTrigger("close");                
            }
            base.ComponentUpdate();
        }

        public void OnTitleScreenFadeOut() {
            Destroy(gameObject);
        }


    }
}

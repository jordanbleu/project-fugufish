using Assets.Source.Components.Level;
using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    /// <summary>
    /// This is very specifically meant for the Death Screen UI object.  No code reuse here.
    /// </summary>
    public class DeathScreenComponent : ComponentBase
    {

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI lives;

        [SerializeField]
        private SceneLoaderComponent sceneLoader;

        private Animator animator;

        public override void ComponentPreStart()
        {
            animator = GetRequiredComponent<Animator>();

            if (GameDataTracker.Lives < 1)
            {
                lives.SetText("");
                animator.SetTrigger("gameover");
            }
            else { 
                lives.SetText(GameDataTracker.Lives.ToString());
            }

            base.ComponentPreStart();
        }

        public void DecrementLives() {
            GameDataTracker.Lives--;
            lives.SetText(GameDataTracker.Lives.ToString());
        }

        public void RetryLevel() {

            var levelComponent = GetRequiredComponent<LevelComponent>(GetRequiredObject("Level"));

            GameDataTracker.FrameToLoadOnSceneLoad = levelComponent.CurrentlyActiveFrame.name;
            sceneLoader.RestartScene();
        }

        public void SetEndText() {
            lives.SetText("The End.");
        }

        public void RestartGame() {
            GameDataTracker.ResetToDefault();
            sceneLoader.LoadScene("Scenes/00_SplashIntros");
            
        }


    }
}

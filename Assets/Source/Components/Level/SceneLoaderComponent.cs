using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.Components.Level
{
    /// <summary>
    /// Shows the loading UI and handles loading the next scene
    /// </summary>
    public class SceneLoaderComponent : ComponentBase
    {
        private Animator animator;
        private string sceneToLoad;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        /// <summary>
        /// Begins the loading screen
        /// </summary>
        public void LoadScene(string name) {
            animator.SetTrigger("show_loading_screen");
            sceneToLoad = name;
        }

        /// <summary>
        /// Call back from the animation timeline when the loading screen is ready for us
        /// </summary>
        public void OnLoadingScreenReady() { 
            StartCoroutine(BeginLoadingScene(sceneToLoad));
        }

        private IEnumerator BeginLoadingScene(string name)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }




    }
}

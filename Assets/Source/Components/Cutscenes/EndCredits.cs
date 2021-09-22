using Assets.Source.Components.Level;
using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes
{
    public class EndCredits : ComponentBase
    {

        [SerializeField]
        private SceneLoaderComponent sceneLoader;

        [SerializeField]
        private GameObject certificate;

        public void Done() {

            // if they collected every single blood vial give a special treat :)
            if (GameDataTracker.GotAllBloodSamples)
            {
                certificate.SetActive(true);
            }
            else {
                // Restarts the whole game
                GameDataTracker.ResetToDefault();
                sceneLoader.LoadScene("Scenes/00_SplashIntros");
            }
        }
    }
}

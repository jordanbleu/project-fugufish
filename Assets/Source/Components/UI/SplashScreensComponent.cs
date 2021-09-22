using Assets.Source.Components.Level;
using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    /// <summary>
    /// This will eventually control the splash screens 
    /// </summary>
    public class SplashScreensComponent : ComponentBase
    {

        [SerializeField]
        private SceneLoaderComponent sceneLoader;

        public void LoadNextScene() { 
            sceneLoader.LoadScene("Scenes/01_Introduction");
        }
    }
}

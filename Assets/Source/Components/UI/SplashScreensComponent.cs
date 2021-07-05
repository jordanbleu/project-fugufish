﻿using Assets.Source.Components.Level;
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

        public override void ComponentStart()
        {
            // For now just go to next scene
            sceneLoader.LoadScene("Scenes/01_Introduction");
            base.ComponentStart();
        }

    }
}

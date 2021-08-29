using Assets.Source.Components.Level;
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

        public void Done() {
            sceneLoader.LoadScene("Scenes/00_SplashIntros");
        }
    }
}

using Assets.Source.Components.Level;
using Assets.Source.Input.Constants;
using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class CertificateOfCompletionComponent : ComponentBase
    {

        [SerializeField]
        private SceneLoaderComponent sceneLoader;

        public override void ComponentUpdate()
        {
            if (Input.IsKeyPressed(InputConstants.K_MENU_ENTER)) {
                // Restarts the whole game
                GameDataTracker.ResetToDefault();
                sceneLoader.LoadScene("Scenes/00_SplashIntros");
            }
            base.ComponentUpdate();
        }

    }
}

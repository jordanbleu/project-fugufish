using Assets.Source.Components.Actor;
using Assets.Source.Components.Level;
using Assets.Source.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class DebugModeComponent : ComponentBase
    {
        [SerializeField]
        private GameObject ui;

        [SerializeField]
        private TextMeshProUGUI enabledText;

        [SerializeField]
        private TextMeshProUGUI frameText;

        [SerializeField]
        private TextMeshProUGUI playerText;

        private bool isDebugEnabled = false;
        private bool isDebugUiEnabled = false;

        private LevelComponent level;
        private ActorComponent player;
        
        [SerializeField]
        private SceneLoaderComponent sceneLoader;

        public override void ComponentAwake()
        {
            level = GetRequiredComponent<LevelComponent>(GetRequiredObject("Level"));
            player = GetRequiredComponent<ActorComponent>(GetRequiredObject("Player"));
            
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.LeftShift) && UnityEngine.Input.GetKeyDown(KeyCode.D)) {
                isDebugEnabled = !isDebugEnabled;
                isDebugUiEnabled = true;
            }

            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.LeftShift) && UnityEngine.Input.GetKeyDown(KeyCode.U))
            {
                isDebugUiEnabled = !isDebugUiEnabled;
            }

            if (isDebugUiEnabled) {
                enabledText.SetText($"Debug mode: {isDebugEnabled}");
                frameText.SetText($"Active Frame: {level.CurrentlyActiveFrame.name}");
                playerText.SetText($"Health: {player.Health} / {player.MaxHealth} -- Stamina: {player.Stamina} / {player.MaxStamina}");
            }

            ui.SetActive(isDebugUiEnabled);

            if (isDebugEnabled) {
                player.Health = player.MaxHealth;
                player.Stamina = player.MaxStamina;
                GameDataTracker.Lives = 3;

                if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                    GameDataTracker.FrameToLoadOnSceneLoad = level.CurrentlyActiveFrame.name;

                    sceneLoader.RestartScene();                    
                }

            }

            base.ComponentUpdate();
        }



    }
}

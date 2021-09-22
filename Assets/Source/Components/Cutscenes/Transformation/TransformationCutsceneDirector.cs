using Assets.Editor.Attributes;
using Assets.Source.Components.Behavior.Humanoid;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Sound;
using Assets.Source.Components.TextWriter;
using Assets.Source.Components.Timer;
using Assets.Source.Components.UI;
using Assets.Source.Strings;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes.Transformation
{
    public class TransformationCutsceneDirector : ComponentBase
    {
        private StringsLoader stringLoader = new StringsLoader();
        private GameObject textWriterQueueInstance;

        [SerializeField]
        private GameObject textWriterQueuePrefab;

        [SerializeField]
        private GameObject uiCanvasObject;


        [SerializeField]
        private PlayerBrainComponent player;

        [SerializeField]
        private GameObject antag;

        [SerializeField]
        private GameObject antag1;

        [SerializeField]
        private GameObject antag2;

        [SerializeField]
        private GameObject boss;

        [SerializeField]
        private GameObject bossBattlePhaseDetector;

        [SerializeField]
        private GameObject bossHealthBar;

        [SerializeField]
        private GameObject killBossDetector;

        [SerializeField]
        private GameObject cam;

        [SerializeField]
        private GameObject cam1;

        [SerializeField]
        private GameObject cam2;

        [SerializeField]
        private GameObject gutsParticleSystem;


        [SerializeField]
        private GameObject finalBossMusic;

        [SerializeField]
        private GameObject goreSound;

        [SerializeField]
        private MusicBoxComponent music;

        [SerializeField]
        private LightningMachineComponent lightning;

        private GameTimerComponent timer;

        [SerializeField]
        [ReadOnly]
        private int stage;

        public override void ComponentPreStart()
        {
            timer = GetRequiredComponent<GameTimerComponent>();
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            switch (stage) {
                case 0:                    
                    // Load the strings
                    stringLoader.Load("Transform/t_00.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;

                case 1:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;

                case 2:
                    // Play music and gore sounds
                    goreSound.SetActive(true);
                    music.FadeInAndPlay();
                    gutsParticleSystem.SetActive(true);
                    var anim = GetRequiredComponent<Animator>(antag);
                    anim.SetBool("is_transform", true);
                    stage++;
                    break;
                case 3:
                    // Load the strings
                    stringLoader.Load("Transform/t_01.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 4:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 5:
                    lightning.Flash();
                    antag.SetActive(false);
                    antag1.SetActive(true);
                    cam.SetActive(false);
                    cam1.SetActive(true);
                    stage++;
                    break;
                case 6:
                    // set timer for 6 seconds 
                    timer.StartTime = 6000;
                    timer.ResetTimer();
                    timer.StartTimer();
                    stage++;
                    break;
                case 7:
                    // waiting for timer
                    break;
                case 8:
                    // Load the strings
                    stringLoader.Load("Transform/t_02.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 9:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;

                case 10:
                    lightning.Flash();

                    antag1.SetActive(false);
                    antag2.SetActive(true);

                    cam1.SetActive(false);
                    cam2.SetActive(true);

                    // set timer for 4 seconds 
                    timer.StartTime = 4000;
                    timer.ResetTimer();
                    timer.StartTimer();
                    stage++;
                    break;

                case 11:
                    // waiting for timer
                    break;


                case 12:
                    // Load the strings
                    stringLoader.Load("Transform/t_03.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;


                case 13:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;

                case 14:
                    // Boss finsihed transformation no more gore sound needed
                    goreSound.SetActive(false);

                    // flash
                    lightning.Flash();
                    cam2.SetActive(false);
                    // activate boss.  He will automatically do the standing animation
                    antag2.SetActive(false);
                    boss.SetActive(true);
                    gutsParticleSystem.SetActive(false);

                    cam2.SetActive(false);
                    stage++;
                    break;
                case 15:
                    // waiting for callback from animation
                    break;
                case 16:
                    // Load the strings
                    stringLoader.Load("Transform/t_04.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 17:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 18:
                    // the fight begins

                    player.SetForceWalk(false);
                    player.SetMovementLock(false);

                    var bossBrain = GetRequiredComponent<BossEnemyBehavior>(boss);
                    bossBrain.SetEnabled(true);
                    Destroy(gameObject);

                    bossBattlePhaseDetector.SetActive(true);
                    bossHealthBar.SetActive(true);
                    killBossDetector.SetActive(true);

                    music.FadeOutAndStop();
                    finalBossMusic.SetActive(true);
                    goreSound.SetActive(false);

                    break;
            }
            base.ComponentUpdate();
        }


        private void AddTextQueue(IEnumerable<string> stringuses)
        {
            textWriterQueueInstance = Instantiate(textWriterQueuePrefab, uiCanvasObject.transform);
            var textQueueComp = GetRequiredComponent<TextWriterQueueComponent>(textWriterQueueInstance);
            textQueueComp.InitQueue(new Queue<string>(stringuses));
        }

        public void IncrementStage() => stage++;
    }
}

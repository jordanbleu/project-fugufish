using Assets.Source.Components.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Objects;
using Assets.Source.Components.TextWriter;
using Assets.Source.Strings;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Components.Cutscenes.PreBoss
{
    public class PreBossCutsceneDirectorComponent : ComponentBase
    {

        private StringsLoader stringLoader = new StringsLoader();
        private GameObject textWriterQueueInstance;

        [SerializeField]
        private GameObject textWriterQueuePrefab;

        [SerializeField]
        private GameObject uiCanvasObject;

        [SerializeField]
        private GameObject antagCam;

        [SerializeField]
        private GameObject bothCam;

        [SerializeField]
        private GameObject playerCam;

        [SerializeField]
        private GameObject antag;

        [SerializeField]
        private DoorComponent doorComponent;

        [SerializeField]
        private PlayerBrainComponent playerBrain;

        [SerializeField]
        private int stage = 0;

        [SerializeField]
        private AudioClip music;

        private SkeletonMecanim antagSkeleton;
        private GameObjectUtilities gameObjectUtilities;

        public override void ComponentAwake()
        {
            antagSkeleton = GetRequiredComponent<SkeletonMecanim>(antag);
            gameObjectUtilities = GetRequiredComponent<GameObjectUtilities>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            switch (stage) {
                case 0:
                    // Load the strings
                    stringLoader.Load("PreBoss/preb_00.xml");
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
                    // show both the dudes
                    bothCam.SetActive(true);
                    
                    // Load the strings.  "not a fan of chairs, i see..."
                    stringLoader.Load("PreBoss/preb_01.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 3:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        // Face the player

                        antagSkeleton.Skeleton.ScaleX = -Mathf.Abs(antagSkeleton.Skeleton.ScaleX);
                        stage++;
                    }
                    break;
                case 4:
                    // Load the strings.  "theres a few things you should know"
                    
                    // Play the music once
                    gameObjectUtilities.PlayExternalAudio(music);
                    
                    stringLoader.Load("PreBoss/preb_02.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 5:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 6:
                    // zoom to antag
                    antagCam.SetActive(true);
                    // Load the strings.  "was it worth it?"
                    stringLoader.Load("PreBoss/preb_03.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 7:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 8:
                    // Load the strings.  "you dont have much time left"
                    stringLoader.Load("PreBoss/preb_04.xml");
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
                    var antagAnimator = GetRequiredComponent<Animator>(antag);
                    antagAnimator.SetBool("is_walking", true);

                    // Face away from the player
                    antagSkeleton.Skeleton.ScaleX = Mathf.Abs(antagSkeleton.Skeleton.ScaleX);

                    // open the door
                    doorComponent.Toggle();

                    // enable the antag cam
                    antagCam.SetActive(true);

                    // set velocity
                    var antagRigidBody = GetRequiredComponent<Rigidbody2D>(antag);
                    antagRigidBody.velocity = new Vector2(2, 0);

                    stage++;
                    break;
                case 11:

                    // wait till antag gets far enough
                    if (antag.transform.position.x > 28) {

                        antagCam.SetActive(false);
                        playerBrain.SetMovementLock(false);
                        playerBrain.SetForceWalk(false);
                        bothCam.SetActive(false);
                        stage++;
                    }
                    break;
                case 12:
                    if (antag.transform.position.x > 32) {
                        Destroy(antag);
                        stage++;
                    }
                    break;
                case 13:
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

    }
}

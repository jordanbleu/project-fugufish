using Assets.Editor.Attributes;
using Assets.Source.Components.Level;
using Assets.Source.Components.TextWriter;
using Assets.Source.Components.Timer;
using Assets.Source.Components.UI;
using Assets.Source.Scene;
using Assets.Source.Strings;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Cutscenes.Intro
{
    [RequireComponent(typeof(IntervalTimerComponent))]
    public class IntroCutsceneDirectorComponent : ComponentBase
    {

        [SerializeField]
        public GameObject fadeObject;

        [SerializeField]
        private GameObject textWriterQueuePrefab;

        [SerializeField]
        private GameObject uiCanvasObject;

        [SerializeField]
        private GameObject playerCam;

        [SerializeField]
        private GameObject centerCam;

        [SerializeField]
        private GameObject rightCam;

        [SerializeField]
        private GameObject antagonistActor;

        [SerializeField]
        private GameObject protagonistActor;

        [SerializeField]
        private GameObject femaleActor;

        [SerializeField]
        private GameObject schnozzActor1;
        
        [SerializeField]
        private GameObject schnozzActor2;

        [SerializeField]
        private GameObject dynamicMenuObject;

        [SerializeField]
        private GameObject titleScreenPrefab;

        [SerializeField]
        private SceneLoaderComponent sceneLoader;
        
        private IntervalTimerComponent timer;
        private Animator fadeAnimator;
        private StringsLoader stringLoader = new StringsLoader();
        private DynamicMenuComponent dynamicMenuComponent;


        private Animator antagonistAnimator;
        private Animator protagonistAnimator;
        private Animator femaleAnimator;
        private ParticleSystem femaleBrainParticleSystem;
        private ParticleSystem protagBrainParticleSystem;

        private SkeletonMecanim antagSkeleton;
        private Rigidbody2D antagRigidBody;

        private int dialogueOption = -1;

        public override void ComponentAwake()
        {
            timer = GetRequiredComponent<IntervalTimerComponent>();
            timer.IsActive = false;
            fadeAnimator = GetRequiredComponent<Animator>(fadeObject);

            antagonistAnimator = GetRequiredComponent<Animator>(antagonistActor);
            protagonistAnimator = GetRequiredComponent<Animator>(protagonistActor);
            femaleAnimator = GetRequiredComponent<Animator>(femaleActor);

            antagSkeleton = GetRequiredComponent<SkeletonMecanim>(antagonistActor);
            antagRigidBody = GetRequiredComponent<Rigidbody2D>(antagonistActor);

            // Localize and prepare the dynamic menu
            // Load the dialog menu strings
            stringLoader.Load("Intro/intro_05_protagOptions.xml");

            dynamicMenuComponent = GetRequiredComponent<DynamicMenuComponent>(dynamicMenuObject);
            dynamicMenuObject.SetActive(false);

            var allMenuItems = new DynamicMenuComponent.MenuItem[] { 
                new DynamicMenuComponent.MenuItem() { Text = stringLoader.Value["protag_scared"], OnItemSelected = new UnityEvent() },
                new DynamicMenuComponent.MenuItem() { Text = stringLoader.Value["protag_smart"], OnItemSelected = new UnityEvent() },
                new DynamicMenuComponent.MenuItem() { Text = stringLoader.Value["protag_brave"], OnItemSelected = new UnityEvent() },
                new DynamicMenuComponent.MenuItem() { Text = stringLoader.Value["protag_silent"], OnItemSelected = new UnityEvent() }
            };

            allMenuItems[0].OnItemSelected.AddListener(() => ChooseOption(0));
            allMenuItems[1].OnItemSelected.AddListener(() => ChooseOption(1));
            allMenuItems[2].OnItemSelected.AddListener(() => ChooseOption(2));
            allMenuItems[3].OnItemSelected.AddListener(() => ChooseOption(3));

            dynamicMenuComponent.InitializeMenu(allMenuItems);

            femaleBrainParticleSystem = GetRequiredComponent<ParticleSystem>(GetRequiredChild("BrainChunkEmitter", femaleActor));
            protagBrainParticleSystem = GetRequiredComponent<ParticleSystem>(GetRequiredChild("BrainChunkEmitter", protagonistActor));


            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            base.ComponentStart();
        }

        [SerializeField]
        [ReadOnly]
        private int stage = 0;

        private GameObject textWriterQueueInstance;
        private GameObject titleScreen;

        public override void ComponentUpdate()
        {

            // This is probably the worst way to write this lol
            switch (stage) {
                case 0:
                    // Load the cold open strings
                    stringLoader.Load("Intro/intro_01_coldOpen.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 1:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance)) {
                        stage++;                    
                    }
                    break;
                case 2:
                    // Trigger the fade in
                    fadeAnimator.SetTrigger("fade_in");
                    stage++;
                    break;
                case 3:
                    // Do nothing.  We're waiting for the animation event to increment the stage 
                    break;
                case 4:
                    // Load strings
                    stringLoader.Load("Intro/intro_02.xml");
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
                    // Use cinemachine to asyncronously pan to other camera
                    centerCam.SetActive(true);
                    // Set a timer for 3 seconds 
                    SetTimer(3000);
                    stage++;
                    break;
                case 7:
                    // Do nothing, we're waiting for the timer to finish
                    break;
                case 8:
                    // Load strings
                    stringLoader.Load("Intro/intro_03.xml");
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
                    // Antag walks over to the girl
                    antagonistAnimator.SetBool("is_walking", true);
                    
                    // Flip the skeleton
                    antagSkeleton.Skeleton.ScaleX = -Mathf.Abs(antagSkeleton.Skeleton.ScaleX);

                    // Move 
                    antagRigidBody.velocity = new Vector2(-1, 0);

                    // if we have reached our destination
                    if (antagonistActor.transform.position.x.IsWithin(0.5f, -7f)) {
                        antagonistAnimator.SetBool("is_walking", false);
                        antagRigidBody.velocity = Vector2.zero;
                        stage++;
                    }
                    break;
                case 11:
                    // Load strings
                    stringLoader.Load("Intro/intro_04.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 12:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 13:
                    // Show the dialog window
                    dynamicMenuObject.SetActive(true);

                    // Next Stage
                    stage++;

                    break;
                case 14:
                    // Do nothing and wait for the menu's callback to increment the stage lol
                    break;
                case 15:
                    
                    // Now decide which dialog to display based on what they picked
                    // inner switch statement, good lord jesus forgive me
                    switch (dialogueOption) {
                        case 0:
                            stringLoader.Load("Intro/intro_06_option1.xml");
                            break;
                        case 1:
                            stringLoader.Load("Intro/intro_06_option2.xml");
                            break;
                        case 2:
                            stringLoader.Load("Intro/intro_06_option3.xml");
                            break;
                        case 3:
                            stringLoader.Load("Intro/intro_06_option4.xml");
                            break;
                    }

                    antagonistAnimator.SetTrigger("aim_weapon");

                    AddTextQueue(stringLoader.Value.Values);

                    stage++;
                    break;
                case 16:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 17:
                    // Shoot the female
                    femaleBrainParticleSystem.Play();
                    antagonistAnimator.SetTrigger("shoot_weapon");
                    femaleAnimator.SetTrigger("get_shot");
                    stage++;
                    break;
                case 18:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 19:
                    // "her blood is on your hands...and so is yours"
                    stringLoader.Load("Intro/intro_07.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    antagonistAnimator.SetTrigger("lower_weapon");
                    stage++;
                    break;
                case 20:
                    // wait for lower weapon to finish
                    break;
                case 21:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 22:
                    // face right 
                    antagSkeleton.Skeleton.ScaleX = Mathf.Abs(antagSkeleton.Skeleton.ScaleX);
                    antagonistAnimator.SetBool("is_walking", true);

                    // Walk back over to the player                     
                    antagRigidBody.velocity = new Vector2(1, 0);

                    // if we have reached our destination
                    if (antagonistActor.transform.position.x.IsWithin(0.5f, -5f))
                    {
                        antagonistAnimator.SetBool("is_walking", false);
                        antagRigidBody.velocity = Vector2.zero;
                        stage++;
                    }
                    break;
                case 23:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 24:
                    antagonistAnimator.SetTrigger("aim_weapon");
                    // "and thus thus cycle of violence continues"
                    stringLoader.Load("Intro/intro_08.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 25:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 26:
                    // Shoot
                    protagBrainParticleSystem.Play();
                    antagonistAnimator.SetTrigger("shoot_weapon");
                    protagonistAnimator.SetTrigger("get_shot");
                    stage++;
                    break;
                case 27:
                    antagonistAnimator.SetTrigger("lower_weapon");
                    stage++;
                    break;
                case 28:
                    // waiting for lower weapon to complete
                    break;
                case 29:
                    // switch to cam revealing the two schnozz enemies
                    rightCam.SetActive(true);
                    // "dont let this happen again"
                    stringLoader.Load("Intro/intro_09.xml");
                    AddTextQueue(stringLoader.Value.Values);
                    stage++;
                    break;
                case 30:
                    // Wait for the player to finish the dialogue thing
                    if (!UnityUtils.Exists(textWriterQueueInstance))
                    {
                        stage++;
                    }
                    break;
                case 31:
                    // face right and walk
                    antagSkeleton.Skeleton.ScaleX = Mathf.Abs(antagSkeleton.Skeleton.ScaleX);
                    antagonistAnimator.SetBool("is_walking", true);

                    // Walk to the right of the screen
                    antagRigidBody.velocity = new Vector2(1, 0);

                    // Once antag walks past the schozzes, they walk too, one by one
                    if (antagonistActor.transform.position.x.IsWithin(0.25f, 1f))
                    {
                        var schnozzActor1Anim = GetRequiredComponent<Animator>(schnozzActor1);
                        var schnozzActor1Rb = GetRequiredComponent<Rigidbody2D>(schnozzActor1);
                        var schnozzActor1Skeleton = GetRequiredComponent<SkeletonMecanim>(schnozzActor1);
                        schnozzActor1Skeleton.Skeleton.ScaleX = Mathf.Abs(schnozzActor1Skeleton.Skeleton.ScaleX);
                        schnozzActor1Anim.SetFloat("horizontal_movement_speed", 1);
                        schnozzActor1Rb.velocity = new Vector2(1, 0);
                    }

                    if (antagonistActor.transform.position.x.IsWithin(0.25f, 3f))
                    {
                        stage++;
                    }

                    break;
                case 32:
                    // Swap back to center cam
                    rightCam.SetActive(false);
                    centerCam.SetActive(true);

                    // spawn the title screen.  
                    titleScreen = Instantiate(titleScreenPrefab, uiCanvasObject.transform);
                    stage++;
                    break;
                case 33:
                    // wait for titleScreen to not exist
                    if (!UnityUtils.Exists(titleScreen)) {
                        stage++;
                    }
                    break;
                case 34:
                    sceneLoader.LoadScene(Scenes.ROOFTOP);
                    break;
            }
            
            base.ComponentUpdate();
        }

        private void AddTextQueue(IEnumerable<string> stringuses) {
            textWriterQueueInstance = Instantiate(textWriterQueuePrefab, uiCanvasObject.transform);
            var textQueueComp = GetRequiredComponent<TextWriterQueueComponent>(textWriterQueueInstance);
            textQueueComp.InitQueue(new Queue<string>(stringuses));
        }

        private void SetTimer(float millisecs) {
            timer.SetInterval(millisecs);
            timer.IsActive = true;
        }

        public void ChooseOption(int option) {
            dialogueOption = option;
            IncrementStage();
        }

        /// <summary>
        /// Increments the stage externally.  More bad code.
        /// </summary>
        public void IncrementStage() => stage++;

    }
}

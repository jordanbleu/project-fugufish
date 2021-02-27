using Assets.Editor.Attributes;
using Assets.Source.Strings;
using Assets.Source.Components.TextWriter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Source.Components.Timer;
using Spine.Unity;
using TMPro;
using Assets.Source.Components.UI;

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
        private GameObject femaleCam;

        [SerializeField]
        private GameObject antagonistActor;

        [SerializeField]
        private GameObject protagonistActor;

        [SerializeField]
        private GameObject femaleActor;


        [SerializeField]
        private GameObject introDialogMenu;

        [SerializeField]
        private GameObject optionButton1;

        [SerializeField]
        private GameObject optionButton2;

        [SerializeField]
        private GameObject optionButton3;

        [SerializeField]
        private GameObject optionButton4;


        private IntervalTimerComponent timer;
        private Animator fadeAnimator;
        private StringsLoader stringLoader = new StringsLoader();
        
        private Animator antagonistAnimator;
        private Animator protagonistAnimator;
        private Animator femaleAnimator;
                
        public override void ComponentAwake()
        {
            timer = GetRequiredComponent<IntervalTimerComponent>();
            timer.IsActive = false;
            fadeAnimator = GetRequiredComponent<Animator>(fadeObject);

            antagonistAnimator = GetRequiredComponent<Animator>(antagonistActor);
            protagonistAnimator = GetRequiredComponent<Animator>(protagonistActor);
            femaleAnimator = GetRequiredComponent<Animator>(femaleActor);

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
                    var antagSkeleton = GetRequiredComponent<SkeletonMecanim>(antagonistActor);
                    antagSkeleton.Skeleton.ScaleX = -Mathf.Abs(antagSkeleton.Skeleton.ScaleX);

                    // Move 
                    var antagRigidBody = GetRequiredComponent<Rigidbody2D>(antagonistActor);
                    antagRigidBody.velocity = new Vector2(-1, 0);

                    // if we have reached our destination
                    if (antagonistActor.transform.position.x.IsWithin(0.5f, -8f)) {
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
                    // Load the dialog menu strings
                    stringLoader.Load("Intro/intro_05_protagOptions.xml");

                    // Grab the text mesh pro elements from each button
                    var option1Text = GetRequiredComponentInChildren<TextMeshProUGUI>(optionButton1);
                    var option2Text = GetRequiredComponentInChildren<TextMeshProUGUI>(optionButton1);
                    var option3Text = GetRequiredComponentInChildren<TextMeshProUGUI>(optionButton2);
                    var option4Text = GetRequiredComponentInChildren<TextMeshProUGUI>(optionButton3);

                    // Apply the localized strings to the button textsssss
                    option1Text.SetText(stringLoader.Value["protag_scared"]);
                    option2Text.SetText(stringLoader.Value["protag_smart"]);
                    option3Text.SetText(stringLoader.Value["protag_brave"]);
                    option4Text.SetText(stringLoader.Value["protag_silent"]);

                    // Activate the dialog selection menu
                    introDialogMenu.SetActive(true);

                    // Grab its animator and trigger the "open" sequence
                    GetRequiredComponent<Animator>(introDialogMenu).SetTrigger("open");

                    // Next Stage
                    stage++;

                    break;
                case 14:
                    // Do nothing and wait for the menu's callback to increment the stage lol
                    break;
                case 15:
                    // Get the selected value.  
                    // TODO: store this somewhere
                    var value = GetRequiredComponent<IntroDialogMenuComponent>().SelectedValue;

                    // Now decide which dialog to display based on what they picked
                    // inner switch statement, good lord jesus forgive me
                    switch (value) {
                        case 1:
                            stringLoader.Load("Intro/intro_06_option1.xml");
                            break;
                        case 2:
                            stringLoader.Load("Intro/intro_06_option2.xml");
                            break;
                        case 3:
                            stringLoader.Load("Intro/intro_06_option3.xml");
                            break;
                        case 4:
                            stringLoader.Load("Intro/intro_06_option4.xml");
                            break;
                    }

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

        /// <summary>
        /// Increments the stage externally.  More bad code.
        /// </summary>
        public void IncrementStage() => stage++;
    }
}

using Assets.Editor.Attributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Timer
{
    /// <summary>
    /// The newer evolution of <seealso cref="IntervalTimerComponent"/> that is a lot less reliant on custom code.
    /// </summary>
    public class GameTimerComponent : ComponentBase
    {
        [SerializeField]
        [Tooltip("Friendly name for debugging only, no real functionality.")]
        private string label = "{New Game Timer}";
        /// <summary>
        /// Friendly name for debugging only, no real functionality.
        /// </summary>
        public string Label { get => label; set => label = value; }

        [SerializeField]
        [Tooltip("The starting time on the timer.")]
        private float startTime = 500f;

        [SerializeField]
        [Tooltip("The current time")]
        private float time;

        public float StartTime { get => startTime; set => startTime = value; }

        [SerializeField]
        [Tooltip("Event invoked when the timer reaches zero.")]
        public UnityEvent onTimerReachZero = new UnityEvent();

        [SerializeField]
        [Tooltip("If true, the timer will begin counting down automatically when the object is awakened.  Otherwise, the StartTimer() method must be called.")]
        private bool startOnAwake = true;

        [SerializeField]
        [ReadOnly]
        private bool isActive = false;

        [SerializeField]
        [Tooltip("Set to true if you want to start the timer at the 'time' you specified.  Otherwise it will be set to max time on awake.")]
        private bool useCustomStartTime = false;

        public override void ComponentAwake()
        {
            if (!useCustomStartTime) {
                time = startTime;
            }

            if (startOnAwake) {
                StartTimer();    
            }
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (isActive) {
                
                time -= (Time.deltaTime * 1000);

                if (time <= 0) {
                    isActive = false;
                    onTimerReachZero?.Invoke();
                }


            }
            base.ComponentUpdate();
        }

        /// <summary>
        /// Reset the timer to its start time
        /// </summary>
        public void ResetTimer()
        {
            time = startTime;
        }

        /// <summary>
        /// Start counting down the timer from the current time
        /// </summary>
        public void StartTimer() {
            isActive = true;
        }

        /// <summary>
        /// Randomizes the current time to between zero and the start time
        /// </summary>
        public void RandomizeCurrentTime() {
            time = UnityEngine.Random.Range(0, startTime);
        }

        

        
    }
}

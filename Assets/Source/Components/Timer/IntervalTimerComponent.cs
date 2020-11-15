using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Timer
{
    /// <summary>
    /// The interval timer component invokes the attached method delegate repeatedly on an interval.
    /// This interval is independent of the framerate of your game.
    /// </summary>
    public class IntervalTimerComponent : ComponentBase
    {
        [Tooltip("The time in milliseconds the interval timer counts down before invoking the unity event")]
        [SerializeField]
        private float interval = 500f;

        [Tooltip("If true, the interval timer will destroy itself after a single interval")]
        [SerializeField]
        private bool _selfDestruct = false;
        public bool SelfDestruct { get => _selfDestruct; set => _selfDestruct = value; }

        [Tooltip("If true, the actual interval time will be a random range UP TO the interval value")]
        [SerializeField]
        private bool _randomize = false;
        public bool Randomize { get => _randomize; set => _randomize = value; }

        [Tooltip("If true, the timer will auto reset and countdown again on interval reached.  If false, will remain inactive until manually reset.")]
        [SerializeField]
        private bool _autoReset = false;
        public bool AutoReset { get => _autoReset; set => _autoReset = value; }

        private float maxInterval;

        /// <summary>
        /// The UnityEvent to subscribe to in order to run code after the time interval is reached.
        /// See <seealso cref="UnityEvent.AddListener(UnityAction)"/> for more details.
        /// </summary>
        public UnityEvent OnIntervalReached;

        /// <summary>
        /// Used to determine if the timer is currently running, defaults to true
        /// </summary>
        public bool IsActive { get; set; } = true;

        public float CurrentTime { get; private set; } = 0.0f;

        public override void ComponentAwake()
        {
            maxInterval = interval;
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            RandomizeInterval();
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if (IsActive)
            {
                CurrentTime += Time.deltaTime * 1000;

                if (CurrentTime >= interval)
                {
                    OnIntervalReached?.Invoke();

                    RandomizeInterval();

                    if (_selfDestruct)
                    {
                        Destroy(gameObject);
                    }

                    if (AutoReset)
                    { 
                        Reset();
                    }

                    IsActive = AutoReset;
                }
            }

            base.ComponentUpdate();
        }

        /// <summary>
        /// Resets the current interval timer and sets the interval time 
        /// </summary>
        /// <param name="interval">time in milliseconds</param>
        public void SetInterval(float interval)
        {
            Reset();
            this.interval = interval;
            maxInterval = interval;
        }

        /// <summary>
        /// Resets the timer to 0 and ensures that its active
        /// </summary>
        public void Reset()
        {
            CurrentTime = 0.0f;
            IsActive = true;
        }

        public float GetInterval()
        {
            return interval;
        }

        private void RandomizeInterval()
        {
            if (Randomize)
            {
                interval = Mathf.RoundToInt(UnityEngine.Random.Range(0, maxInterval));
            }
        }
    }
}

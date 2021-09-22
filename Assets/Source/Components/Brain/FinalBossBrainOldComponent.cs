using Assets.Source.Components.Animation;
using Assets.Source.Components.Brain.Base;
using Assets.Source.Components.Cutscenes.Transformation;
using Assets.Source.Components.Timer;
using System;
using UnityEngine;

namespace Assets.Source.Components.Brain
{
    public class FinalBossBrainOldComponent : CommonPhysicsComponent
    {
        private const float MELEE_HEIGHT_RANGE = 3f;

        // false while cutscene is running.
        private bool brainIsEnabled = true; // TODO-set to false for actual game


        [SerializeField]
        private TransformationCutsceneDirector cutscene;

        #region melee properties
        [SerializeField]
        [Header("Follow player Properties")]
        [Tooltip("How up close and personal the actor gets to the player")]
        private float rangeFromPlayer = 6f;

        [SerializeField]
        private float closeRange = 3f;



        [SerializeField]
        [Tooltip("How closely the actor needs to be to the player's position +/- range")]
        private float precision = 0.25f;

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMin;

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMax;
        #endregion

        #region references
        [SerializeField]
        [Tooltip("Drag the brain timer here")]
        private GameTimerComponent brainTimer;

        [SerializeField]
        private GameObject player;
                
        [SerializeField]
        private float moveSpeed = 2;
        #endregion

        private bool isAttacking = false;
        private bool isDamageEnabled = false;

        private FinalBossAnimatorComponent animator;

        public override void ComponentPreStart()
        {
            animator = GetRequiredComponent<FinalBossAnimatorComponent>();
            base.ComponentPreStart();
        }

        public override void ComponentStart()
        {
            base.ComponentStart();
        }

        public void BrainUpdate() {

            if (brainIsEnabled) {

                // Should Shoot at the player
                if (player.transform.position.y > MELEE_HEIGHT_RANGE)
                {
                    ShootAtPlayer();
                }
                // Should melee the player
                else if (PlayerIsInRange() && !isAttacking) {

                    if (PlayerIsInCloseRange())
                    {
                        // Do a devastating close attack
                        var whatToDo = UnityEngine.Random.Range(0, 4);

                        if (whatToDo <= 3)
                        {
                            // A basic downward attack
                            animator.BodySmash();
                        }
                        else
                        {
                            // The more slow / devastating attack (less likely to happen)
                            animator.DownwardSmash();
                        }
                    }
                    else {
                        var whatToDo = UnityEngine.Random.Range(0, 4);
                        // 50/50 chance here
                        if (whatToDo < 2)
                        {
                            animator.OverheadSlice();
                        }
                        else {
                            animator.SwordSlice();
                        }
                    }
                
                }

                // Update + restart the timer
                brainTimer.ResetTimer();
                brainTimer.StartTime = UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax);
                brainTimer.StartTimer();
            }
        }

        public override void ComponentUpdate()
        {
            // Move towards the player always
            if (!PlayerIsInRange()) {

                // if player is to my right, seek out the player's left side
                if (player.transform.position.x > transform.position.x)
                {
                    FootVelocity = MoveTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }
                else
                {
                    FootVelocity = MoveTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }

            }
            base.ComponentUpdate();
        }

        private Vector2 MoveTowards(Vector2 vector2, float precision)
        {
            if (!isAttacking)
            {
                // No need for intelligent movement.  Final boss battle has no obstacles.
                if (transform.position.x.IsWithin(precision, vector2.x))
                {
                    if (transform.position.x > vector2.x)
                    {
                        return new Vector2(moveSpeed, CurrentVelocity.y);
                    }
                    else
                    {
                        return new Vector2(-moveSpeed, CurrentVelocity.y);
                    }
                }
            }
            return new Vector2(0, CurrentVelocity.y);
        }

        private void ShootAtPlayer()
        {
            // todo: implement
        }

        // Player is in range for a melee attack
        private bool PlayerIsInRange() => 
            (player.transform.position.x > (transform.position.x - rangeFromPlayer) && player.transform.position.x < (transform.position.x + rangeFromPlayer));

        private bool PlayerIsInCloseRange() =>
            (player.transform.position.x > (transform.position.x - closeRange) && player.transform.position.x < (transform.position.x + closeRange));

        public void EnableBrain()
        {
            brainIsEnabled = true;
            // Update + restart the timer
            brainTimer.ResetTimer();
            brainTimer.StartTime = UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax);
            brainTimer.StartTimer();
        }

        #region animation callbacks

        public void OnStandUp() {
            cutscene.IncrementStage();
        }

        public void OnAttackBegin() {
            isAttacking = true;
        }

        public void OnAttackEnd() {
            isAttacking = false;
        }

        public void OnDamageBegin() {
            isDamageEnabled = true;
        }

        public void OnDamageEnd() {
            isDamageEnabled = false;
        }
        #endregion

    }
}

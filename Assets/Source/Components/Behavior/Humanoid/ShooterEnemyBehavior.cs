﻿using Assets.Editor.Attributes;
using Assets.Source.Components.Actor;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Timer;
using Assets.Source.Enums;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Source.Components.Animation.ShooterSkeletonAnimatorComponent;

namespace Assets.Source.Components.Behavior.Humanoid
{
    /// <summary>
    /// Shooter Enemy works in 2 parts:
    /// <para> - He will seek out a given position out of a list of possible positions </para>
    /// <para> - Once he is at the position, he will fire at the player. </para>
    /// </summary>
    public class ShooterEnemyBehavior : HumanoidBehaviorBase
    {

        [Header("Shooter Behavior Properties")]
        [SerializeField]
        [Tooltip("Possible positions to move the shooter to.  When the player gets too close, the shooter will move over to the next closest position.")]
        private List<ShooterPosition> Positions = new List<ShooterPosition>();

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMin;

        [SerializeField]
        [Tooltip("Dictates how quickly the AI makes decisions.  Lower times make the AI faster at reacting / attacking, etc.  Will randomize between the min and max values.")]
        private float thinkTimeMax;

        [SerializeField]
        [Tooltip("Adjusts the threshold for the actor to aim upwards vs downwards.  Adjust so the red lines make sense in the gizmo")]
        private float aimHeight = 0.75f;

        [SerializeField]
        [Tooltip("How fast the bullet moves when shot.")]
        private float bulletSpeed = 5;

        [SerializeField]
        [Tooltip("Object to spawn as the bullet")]
        private GameObject bulletPrefab;

        private PlayerBrainComponent playerBrain;
        private IntervalTimerComponent brainTimer;
        private MeleeComponent meleeCollider;

        private bool shouldDodge = false;
        private bool isAttacking = false;

        // i'm being lazy here and creating a second reference to the animator
        // but in reality this should probably be a type parameter for the animator
        private ShooterSkeletonAnimatorComponent shooterAnimator;

        [SerializeField]
        [ReadOnly]
        private ShooterPosition currentSeekedPosition;


        public override void ComponentStart()
        {
            if (!UnityUtils.Exists(bulletPrefab))
            {
                Debug.LogWarning($"No bullet prefab specified for {gameObject.name}!");
            }

            if (!Positions.Any()) {
                Debug.LogError($"Enemy shooter named '{gameObject.name}' doesn't have any predefined positions and his behavior will not work right.");
            }

            var brain = GetRequiredComponent<HumanoidNPCBrainComponent>();
            brain.attackByPlayer.AddListener(OnAttackedByPlayer);
            brain.attackBegin.AddListener(OnAttackBegin);
            brain.attackEnd.AddListener(OnAttackEnd);
            brain.damageEnable.AddListener(OnDamageEnable);
            brain.damageDisable.AddListener(OnDamageDisable);

            var brainTimerPrefabTemp = new GameObject();
            brainTimerPrefabTemp.AddComponent<IntervalTimerComponent>();
            var inst = Instantiate(brainTimerPrefabTemp, transform);
            inst.name = "BrainTimer";

            brainTimer = GetRequiredComponent<IntervalTimerComponent>(inst);
            brainTimer.Label = "BrainTimerComponent";
            brainTimer.SetInterval((int)UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax));
            brainTimer.SelfDestruct = false;
            brainTimer.Randomize = false;
            brainTimer.AutoReset = true;
            brainTimer.OnIntervalReached.AddListener(BrainUpdate);

            shooterAnimator = GetRequiredComponent<ShooterSkeletonAnimatorComponent>();

            Destroy(brainTimerPrefabTemp);

            currentSeekedPosition = FindNearestShooterPosition();

            base.ComponentStart();
        }

        private ShooterPosition FindNearestShooterPosition() => 
            Positions.OrderBy(pos => Mathf.Abs(transform.position.x - pos.Position.x)).First();

        // This will calculate the Next farthest position from the player, preferring moving in the opposite direction
        private ShooterPosition FindSecondNearestShooterPositionAwayFromPosition(float playerX) => 
            Positions.Count() == 1 ? 
            FindNearestShooterPosition() : 
            Positions.OrderBy(pos => (-(Mathf.Sign(playerX)) * Mathf.Abs(transform.position.x - pos.Position.x))).Skip(1).First();

        public override void ComponentAwake()
        {
            base.ComponentAwake();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>();
        }

        private void OnAttackedByPlayer()
        {
            if (playerBrain.ActiveAttack == AttackTypes.Uppercut)
            {
                AddRigidBodyForce(0, 10f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                AddImpact(15f, 0);
            }
            else
            {
                AddImpact(-15f, 0);
            }

            // todo: add ladder climbing someday
            animator.DamageFront();

        }

        public override void ComponentFixedUpdate() 
        {
            // If we are not at our position, move towards the position
            if (!IsNearEnoughToPosition())
            {
                FootVelocity = MoveIntelligentlyTowards(currentSeekedPosition.Position, 0.5f);
            }
            else {
                // else, simply face the player
                animator.FaceTowardsPosition(player.transform.position);
                FootVelocity = new Vector2(0, CurrentVelocity.y);
            }

            base.ComponentFixedUpdate();
        }



        // Called from the interval timer
        private void BrainUpdate()
        {
            if (isActiveAndEnabled)
            {
                // if we're at our position, fire a bullet towards the player
                if (IsNearEnoughToPosition()) { 
                    Attack();
                }

                // Additionally, if the player is in our area, pick a new position
                if (player.transform.position.x.IsWithin(currentSeekedPosition.Range, currentSeekedPosition.Position.x)) {
                    currentSeekedPosition = FindSecondNearestShooterPositionAwayFromPosition(player.transform.position.x);                         
                }               
                
                brainTimer.SetInterval((int)UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax));
            }
        }

        private bool IsNearEnoughToPosition() => transform.position.x.IsWithin(currentSeekedPosition.Range/2, currentSeekedPosition.Position.x);

        private void Attack()
        {
            AimAtPlayer();
            animator.Attack();
        }

        private void AimAtPlayer()
        {
            animator.FaceTowardsPosition(player.transform.position);

            if (player.transform.position.y > transform.position.y + aimHeight)
            {
                shooterAnimator.Direction = ShootDirection.Up;
            }
            else if (player.transform.position.y < transform.position.y - aimHeight)
            {
                shooterAnimator.Direction = ShootDirection.Down;
            }
            else
            {
                shooterAnimator.Direction = ShootDirection.Forward;
            }
        }

        #region Event Handlers
        private void OnAttackBegin()
        {
            var bulletInst = Instantiate(bulletPrefab, transform.parent);
            bulletInst.transform.position = transform.position;

            // calculate velocity towards the player
            var vel = (player.transform.position - transform.position).normalized * bulletSpeed;

            var bulletRigidBody = bulletInst.GetComponent<Rigidbody2D>();
            bulletRigidBody.velocity = vel;
        }

        private void OnAttackEnd()
        {
            isAttacking = false;
        }

        private void OnDamageEnable()
        {
            // Add force to the actor 
            if (animator.SkeletonIsFlipped)
            {
                AddImpact(-dodgeSpeed, 0); 
            }
            else
            {
                AddImpact(dodgeSpeed, 0);
            }

            meleeCollider.IsDamageEnabled = true;
        }

        private void OnDamageDisable()
        {
            meleeCollider.IsDamageEnabled = false;
        }
        #endregion

        [Serializable]
        public struct ShooterPosition {

            [Tooltip("The position to move to")]
            public Vector2 Position;

            [Tooltip("The color to show for the gizmo.  Used for debugging / visualizing paths.")]
            public Color GizmoColor;

            [Tooltip("The range that the shooter is allowed to be within the position")]
            public float Range;
        }

        public override void DrawAdditionalGizmosSelected()
        {
            if (Positions.Any()) { 
                foreach (var position in Positions) {
                    Gizmos.color = position.GizmoColor;
                    Gizmos.DrawWireCube(position.Position, new Vector2(position.Range,3));                
                }            
            }

            // red lines show the aiming heights
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y + aimHeight));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y - aimHeight));
            Gizmos.color = UnityUtils.Color(225, 100, 100);
            base.DrawAdditionalGizmosSelected();
        }

    }


}

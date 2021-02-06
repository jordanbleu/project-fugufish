using Assets.Source.Components.Actor;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Projectile;
using Assets.Source.Components.Timer;
using Assets.Source.Enums;
using System;
using UnityEngine;
using static Assets.Source.Components.Animation.ShooterSkeletonAnimatorComponent;

namespace Assets.Source.Components.Behavior.Humanoid
{
    /// <summary>
    /// Shooter enemy will hold its position until the enemy gets too close, in which case it will run
    /// </summary>
    public class ShooterEnemyBehaviorOldBad : HumanoidBehaviorBase
    {
        [SerializeField]
        [Header("Shooter Behavior Properties")]
        [Tooltip("How close the player has to be for the enemy to run to a new position")]
        private float runAwayDistance = 2f;

        [SerializeField]
        [Tooltip("How closely the actor needs to be to the destination position")]
        private float precision = 0.25f;

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
        private GameObject bulletPrefab;

        private PlayerBrainComponent playerBrain;
        private IntervalTimerComponent brainTimer;
        private ActorComponent actor;

        // i'm being lazy here and creating a second reference to the animator
        // but in reality this should probably be a type parameter for the animator
        private ShooterSkeletonAnimatorComponent shooterAnimator;

        private float destinationX;


        public override void ComponentStart()
        {
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
            actor = GetRequiredComponent<ActorComponent>();

            var brain = GetRequiredComponent<HumanoidNPCBrainComponent>();
            brain.attackByPlayer.AddListener(OnAttackedByPlayer);
            brain.attackBegin.AddListener(OnAttackBegin);

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

            Destroy(brainTimerPrefabTemp);

            destinationX = transform.position.x;

            shooterAnimator = GetRequiredComponent<ShooterSkeletonAnimatorComponent>();

            if (!UnityUtils.Exists(bulletPrefab)) {
                Debug.LogWarning($"No bullet prefab specified for {gameObject.name}!");
            }

            base.ComponentStart();
        }

        private void BrainUpdate()
        {
            UpdateDestination();

            if (actor.IsAlive())
            {
                if (transform.position.x.IsWithin(precision, destinationX))
                {
                    AimAtPlayer();
                    //shooterAnimator.Shoot();
                } 
            }
        }

        private void UpdateDestination()
        {
            var distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);

            if (distanceFromPlayer > runAwayDistance)
            {
                var footCenter = GetFeetCenter();
                // enemy is to the left of the player
                if (transform.position.x < player.transform.position.x)
                {
                    destinationX = UnityEngine.Random.Range(player.transform.position.x - (runAwayDistance * 2), player.transform.position.x - (runAwayDistance));
                }
                else
                {
                    destinationX = UnityEngine.Random.Range(player.transform.position.x + (runAwayDistance * 2), player.transform.position.x + (runAwayDistance));
                }

                // this is hard to read so just trust that it works
                var wallHit = Physics2D.Raycast(new Vector2(transform.position.x, footCenter.y - lookBelowDistance), 
                                                new Vector2(Mathf.Sign(destinationX), footCenter.y), 
                                                Mathf.Abs(transform.position.x - destinationX), groundLayers, -999, 999);

                if (UnityUtils.Exists(wallHit.collider)) {
                    // if an object is in the way, set the destination to the edge of the wall
                    destinationX = wallHit.point.x;
                }


            }
        }

        public override void ComponentFixedUpdate()
        {
            if (!transform.position.x.IsWithin(runAwayDistance, destinationX)) {
                FootVelocity = MoveIntelligentlyTowards(new Vector2(destinationX, transform.position.y), precision);
            }
            base.ComponentFixedUpdate();
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

        private void OnAttackBegin()
        {
            var bulletInst = Instantiate(bulletPrefab, transform.parent);
            bulletInst.transform.position = transform.position;

            // calculate velocity towards the player
            var vel = (player.transform.position - transform.position).normalized * bulletSpeed;

            var bulletRigidBody = bulletInst.GetComponent<Rigidbody2D>();
            bulletRigidBody.velocity = vel;
            
        }

        private void OnAttackedByPlayer()
        {
            if (actor.IsAlive())
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
        }


        public override void DrawAdditionalGizmosSelected()
        {
            // red lines show the aiming heights
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x+1, transform.position.y+aimHeight));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x+1, transform.position.y - aimHeight));
            Gizmos.color = UnityUtils.Color(225, 100, 100);
            
            // weird bandaid / salmon kinda color is the run away range
            Gizmos.DrawLine(new Vector2(transform.position.x - runAwayDistance, transform.position.y), 
                new Vector2(transform.position.x + runAwayDistance, transform.position.y));

            Gizmos.color = UnityUtils.Color(120, 50, 120);
            Gizmos.DrawCube(new Vector3(destinationX, transform.position.y), new Vector2(1, 1));

            base.DrawAdditionalGizmosSelected();
        }
       
    }
}

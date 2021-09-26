using Assets.Source.Components.Actor;
using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Components.Timer;
using UnityEngine;
using static Assets.Source.Components.Animation.ShooterSkeletonAnimatorComponent;

namespace Assets.Source.Components.Behavior.Humanoid
{
    public class TurretBehavior : HumanoidBehaviorBase
    {

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

        [SerializeField]
        [Tooltip("If true, the turret can only shoot to the right.  If false, left.")]
        private bool IsFacingRight = true;
        

        private PlayerBrainComponent playerBrain;
        private IntervalTimerComponent brainTimer;
        private MeleeComponent meleeCollider;

        // i'm being lazy here and creating a second reference to the animator
        // but in reality this should probably be a type parameter for the animator
        private ShooterSkeletonAnimatorComponent shooterAnimator;

        private ShooterSoundEffects sound;

        public override void ComponentAwake()
        {
            sound = GetRequiredComponent<ShooterSoundEffects>();
            if (!UnityUtils.Exists(player))
            {
                Debug.LogError($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
                throw new UnityException($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
            }

            base.ComponentAwake();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
            meleeCollider = GetRequiredComponentInChildren<MeleeComponent>();
        }


        public override void ComponentStart()
        {
            if (!UnityUtils.Exists(bulletPrefab))
            {
                Debug.LogWarning($"No bullet prefab specified for {gameObject.name}!");
            }


            var brain = GetRequiredComponent<HumanoidNPCBrainComponent>();
            brain.attackBegin.AddListener(OnAttackBegin);
            brain.attackByPlayer.AddListener(OnAttackedByPlayer);

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

            if (!IsFacingRight) {
                shooterAnimator.FaceTowardsPosition(transform.position.x-1);
            }

            Destroy(brainTimerPrefabTemp);

            base.ComponentStart();
        }

        // Called from the interval timer
        private void BrainUpdate()
        {
            if (isActiveAndEnabled)
            {
            if (player.transform.position.x > transform.position.x || !IsFacingRight)
            {
                Attack();                                   
            }
                brainTimer.SetInterval((int)UnityEngine.Random.Range(thinkTimeMin, thinkTimeMax));
            }
        }

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
            sound.Shot1();

            var bulletInst = Instantiate(bulletPrefab, transform.parent);
            bulletInst.transform.position = transform.position;

            // calculate velocity towards the player
            var vel = (player.transform.position - transform.position).normalized * bulletSpeed;

            var bulletRigidBody = bulletInst.GetComponent<Rigidbody2D>();
            bulletRigidBody.velocity = vel;
        }


        #endregion

        private void OnAttackedByPlayer()
        {
            animator.DamageFront();
        }

        public override void DrawAdditionalGizmosSelected()
        {
            // red lines show the aiming heights
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y + aimHeight));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + 1, transform.position.y - aimHeight));
            Gizmos.color = UnityUtils.Color(225, 100, 100);
            base.DrawAdditionalGizmosSelected();
        }
    }
}

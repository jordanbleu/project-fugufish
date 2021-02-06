using Assets.Source.Components.Behavior.Base;
using Assets.Source.Components.Brain;
using Assets.Source.Enums;
using System;
using UnityEngine;

namespace Assets.Source.Components.Behavior.Humanoid
{
    public class FollowPlayerHumanoidBehavior : HumanoidBehaviorBase
    {
        [SerializeField]
        [Header("Follow player Properties")]
        [Tooltip("How up close and personal the actor gets to the player")]
        private float rangeFromPlayer = 2f;


        [SerializeField]
        [Tooltip("How closely the actor needs to be to the player's position +/- range")]
        private float precision = 0.25f;

        private PlayerBrainComponent playerBrain;

        public override void ComponentStart()
        {
            var brain = GetRequiredComponent<HumanoidNPCBrainComponent>();
            brain.attackByPlayer.AddListener(OnAttackedByPlayer);
            base.ComponentStart();
        }

        public override void ComponentAwake()
        {
            base.ComponentAwake();
            playerBrain = GetRequiredComponent<PlayerBrainComponent>(player);
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
            else {
                AddImpact(-15f, 0);
            }
        }

        public override void ComponentFixedUpdate()
        {

            MoveTowardsPlayer();            
            base.ComponentFixedUpdate();
        }

        private void MoveTowardsPlayer()
        {
            // if player is outside actor's range of motion, or theyre in the air
            if (!PlayerIsInRange())
            {
                // if player is to my right, seek out the player's left side
                if (player.transform.position.x > transform.position.x)
                {
                    FootVelocity = MoveIntelligentlyTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }
                else
                {
                    FootVelocity = MoveIntelligentlyTowards(new Vector2(player.transform.position.x - rangeFromPlayer, transform.position.y), precision);
                }
            }
            else
            {
                // else, simply face the player
                animator.FaceTowardsPosition(player.transform.position);
                FootVelocity = new Vector2(0, CurrentVelocity.y);
            }

        }

        private bool PlayerIsInRange() => (player.transform.position.x > (transform.position.x - rangeFromPlayer) && player.transform.position.x < (transform.position.x + rangeFromPlayer));



    }


}

using Assets.Source.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Player.AnimationBehaviors
{
    public class PlayerSwingAnimationBehavior : StateMachineBehaviour
    {
        private PlayerComponent playerComponent;
        private Rigidbody2D rigidbody;

        private float currentSpeed = 0f;
        private float decelerationRate = 0.1f;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerComponent = animator.gameObject.GetComponent<PlayerComponent>();
            rigidbody = animator.gameObject.GetComponent<Rigidbody2D>();

            playerComponent.OverrideMovement = true;
            currentSpeed = playerComponent.SwingMovementSpeed;
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            currentSpeed = currentSpeed.Normalize(decelerationRate, 0);

            if (playerComponent.DirectionFacing == Directions.Up)
            {
                rigidbody.velocity = new Vector2(0, currentSpeed);
            }
            else 
            {
                rigidbody.velocity = new Vector2(0, -currentSpeed);
            }

            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerComponent.OverrideMovement = false;
            base.OnStateExit(animator, stateInfo, layerIndex);
        }

    }
}

using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Behavior
{
    public class DoNothingHumanoidBehavior : HumanoidBehaviorBase
    {
        public override void ComponentAwake()
        {
            if (!UnityUtils.Exists(player)) {
                Debug.LogError($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
                throw new UnityException($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
            }

            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = 0f;
            animator.VerticalMoveSpeed = 0f;
            base.ComponentUpdate();
        }
    }
}

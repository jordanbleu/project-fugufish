using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Behavior
{
    public class DoNothingHumanoidBehavior : HumanoidBehaviorBase
    {
        public override void ComponentPreStart()
        {
            if (!UnityUtils.Exists(player)) {
                Debug.LogError($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
                throw new UnityException($"Object '{gameObject.name}' needs a reference to the player object on component 'DoNothingHumanoidBehavior'");
            }

            base.ComponentPreStart();

            if (!UnityUtils.Exists(animator)) {
                throw new UnityException($"Can't find a humanoid skeleton animator on object {gameObject.name}");
            }
        }

        public override void ComponentUpdate()
        {
            FootVelocity = new Vector2(0, CurrentVelocity.y);
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = 0f;
            animator.VerticalMoveSpeed = 0f;
            base.ComponentUpdate();
        }
    }
}

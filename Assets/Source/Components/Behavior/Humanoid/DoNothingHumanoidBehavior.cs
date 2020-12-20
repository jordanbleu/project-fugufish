using Assets.Source.Components.Animation;
using Assets.Source.Components.Behavior.Base;
using Spine.Unity;
using UnityEngine;

namespace Assets.Source.Components.Behavior
{
    public class DoNothingHumanoidBehavior : HumanoidBehaviorBase
    {
        public override void ComponentUpdate()
        {
            animator.IsGrounded = IsGrounded;
            animator.HorizontalMoveSpeed = 0f;
            animator.VerticalMoveSpeed = 0f;
            base.ComponentUpdate();
        }
    }
}

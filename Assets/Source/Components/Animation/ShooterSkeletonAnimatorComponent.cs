namespace Assets.Source.Components.Animation
{
    public class ShooterSkeletonAnimatorComponent : HumanoidSkeletonAnimatorComponent
    {

        public enum ShootDirection { 
            Up = 1,
            Forward = 0,
            Down = -1,
        }

        public ShootDirection Direction { get; set; }

        public override void UpdateHumanoidAnimationParameters()
        {
            animator.SetFloat("shoot_direction", (float)Direction);
            base.UpdateHumanoidAnimationParameters();
        }

    }
}

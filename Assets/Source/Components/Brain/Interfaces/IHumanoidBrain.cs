namespace Assets.Source.Components.Brain.Interfaces
{
    /// <summary>
    /// Requires implementation of the humanoid animation events and whatnot
    /// </summary>
    public interface IHumanoidBrain
    {
        void OnAttackBegin();

        void OnUppercutBegin();

        void OnGroundPoundBegin();

        void OnAttackEnd();

        void OnDamageEnable();

        void OnDamageDisable();

        void OnGroundPoundLanded();

    }
}

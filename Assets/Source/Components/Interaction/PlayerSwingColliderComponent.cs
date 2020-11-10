using Assets.Source.Input.Constants;
using UnityEngine;

namespace Assets.Source.Components.Interaction
{
    /// <summary>
    /// Handles player's melee attack logic and animations
    /// </summary>
    public class PlayerSwingColliderComponent : ComponentBase
    {

        // Components 
        private Animator animator;
        
        public bool IsActive { get; private set; }

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        /// <summary>
        /// Triggered via attack animation
        /// </summary>
        public void EnableDamage() => IsActive = true;

        /// <summary>
        /// Triggered via attack animation
        /// </summary>
        public void DisableDamage() => IsActive = false; 

    }
}

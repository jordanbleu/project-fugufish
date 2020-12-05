using UnityEngine;

namespace Assets.Source.Components.Brain.Base
{
    /// <summary>
    /// Handles common physics for platforming type games in an abstract fashion.  
    /// If an actor exists in the physical world, it probably should inherit from this class.
    /// </summary>
    public abstract class CommonPhysicsComponent : ComponentBase
    {
        /// <summary>
        /// This is the velocity the actor is walking or jumping
        /// </summary>
        protected Vector2 FootVelocity { get; set; }

        /// <summary>
        /// This is the velocity the actor is moving due to environmental factors, such as wind, water, etc
        /// </summary>
        protected Vector2 EnvironmentalVelocity { get; set; }

        /// <summary>
        /// This is the velocity the actor is moving due to an impact.  The values self-stabilize smoothly.
        /// </summary>
        protected Vector2 ImpactVelocity { get; set; }







    }
}

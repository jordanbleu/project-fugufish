﻿using UnityEngine;

namespace Assets.Source.Components.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class ForceComponent : ComponentBase
    {

        [Tooltip("The amount of force applied to objects.  Can be negative!")]
        public Vector2 Amount = new Vector2(0.1f, 0);

        [Tooltip("If true, force will only apply if the player is grounded.")]
        public bool IsGround = false;
    }
}

using System;
using UnityEngine;

namespace Assets.Source.Math
{
    /// <summary>
    /// Used to represent a 2D square in 2D space
    /// </summary>
    [Serializable]
    public struct Square
    {
        public Vector2 Center;
        public float Width;
        public float Height;
    }
}

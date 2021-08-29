using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Scene
{
    /// <summary>
    /// Tracks all the game data while the game is running.  
    /// </summary>
    public static class GameDataTracker
    {

        /// <summary>
        /// Tracks the last frame that the player was on.  Tells the scene loader to 
        /// set an active frame based on this information.
        /// </summary>
        public static string FrameToLoadOnSceneLoad { get; set; } = null;

        /// <summary>
        /// Tracks how many lives / retries the player currently has.
        /// </summary>
        public static int Lives { get; set; } = 3;

        /// <summary>
        /// Which frame the player last died on.  Notice, because of this, frame names need to be unique.
        /// </summary>
        public static string LastDeathFrameName { get; set; } = null;

        /// <summary>
        /// Tracks the position of the player when he is standing on the ground only
        /// </summary>
        public static Vector2? LastGroundedPosition { get; set; } = null;


        /// <summary>
        /// Tracks the last grounded position at the time of the player's death
        /// </summary>
        public static Vector2? LastGroundedDeathPosition { get; set; } = null;



        internal static void ResetToDefault()
        {
            Lives = 3;
            FrameToLoadOnSceneLoad = null;
            LastDeathFrameName = null;
            LastGroundedPosition = null;
        }
    }
}

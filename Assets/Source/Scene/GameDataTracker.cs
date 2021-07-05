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
        public static GameObject FrameToLoadOnSceneLoad { get; set; }

        /// <summary>
        /// Tracks how many lives / retries the player currently has.
        /// </summary>
        public static int Lives { get; set; } = 3;

        internal static void ResetToDefault()
        {
            Lives = 3;
            FrameToLoadOnSceneLoad = null;
        }
    }
}

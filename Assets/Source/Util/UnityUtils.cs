using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Adds some of that good syntactic sugar and spice
    /// </summary>
    public static class UnityUtils
    {

        /// <summary>
        /// This is the proper way to check if an object is null according to some random
        /// guy on the internet.  It's apparently due to the way that C# classes wrap 
        /// C++ objects in memory.
        /// </summary>
        /// <param name="unityObject">Object to check</param>
        public static bool Exists(Object unityObject) => unityObject != null && !unityObject.Equals(null);

    }
}

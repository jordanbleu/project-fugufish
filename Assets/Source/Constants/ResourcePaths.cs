namespace Assets.Source.Constants
{
    /// <summary>
    /// Paths to required resources should be defined here
    /// </summary>
    public static class ResourcePaths
    {
        /// <summary>
        /// Points to the full path to the strings folder
        /// </summary>
        public static string StringsDirectory => $"{UnityEngine.Application.dataPath}/Strings"; 


        /// <summary>
        /// This should point to the resources folder independent of platform (hopefully)
        /// </summary>
        public static string ResourcesDirectory => $"{UnityEngine.Application.dataPath}/Resources";

        /// <summary>
        /// Points to the prefabs folder.  Not fully qualified file path to the directory 
        /// (This string is literally just "Prefabs")
        /// <para>Use this alongside instantiating prefabs</para>
        /// </summary>
        public static string PrefabsFolder => $"Prefabs";

        public static string SoundFXFolder => $"Sounds/SoundFX";
        public static string MusicFolder => $"Sounds/Music";


    }
}

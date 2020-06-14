using Assets.Source.Configuration.Base;
using Assets.Source.Configuration.Constants;
using System;

namespace Assets.Source.Configuration
{
    [Serializable]
    public class SystemConfiguration : ConfigurationBase
    {
        /// <summary>
        /// The language code that determines the folder where string resources are loaded from
        /// </summary>
        public string Language { get; set; } = ConfigurationConstants.Languages.en.ToString();

        /// <summary>
        /// The X Resolution of the UI. 
        /// </summary>
        public int UIResolutionX { get; set; } = 480;

        /// <summary>
        /// The Y resolution of the UI
        /// </summary>
        public int UIResolutionY { get; set; } = 270;



    }
}

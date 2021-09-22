using Assets.Source.Configuration;
using Assets.Source.Strings.Base;
using UnityEngine;

namespace Assets.Source.Strings
{
    /// <summary>
    /// Used to load a full set of strings from an xml resource.  Keeps strings cached.
    /// </summary>
    public class StringsLoader : StringsLoaderBase
    {
        protected override string GetStringsDir()
        {
            return $"{Application.streamingAssetsPath}/Strings/";
            //return ResourcePaths.StringsDirectory;
        }

        protected override string GetLanguageCode()
        {
            return ConfigurationRepository.SystemConfiguration.Language;
        }
    }
}

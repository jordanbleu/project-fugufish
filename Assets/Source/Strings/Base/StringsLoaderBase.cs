using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Assets.Source.Strings.Base
{
    public abstract class StringsLoaderBase
    {
        // The file name we loaded
        private string resourcePath;

        // The cached dictionary of strings
        public Dictionary<string, string> Value { get; private set; }

        /// <summary>
        /// Call load to return a dictionary of strings. 
        /// </summary>
        /// <param name="path">The path after the 'strings/en' folder (or whatever the language code is).  Include the .xml</param>
        public void Load(string path)
        {
            StringResourceList strings;
            strings = DeserializeStrings(path);
            ConvertToDictionary(strings);
        }

        // Converts the deserialized string resource object into the cached dictionary
        private void ConvertToDictionary(StringResourceList stringResources)
        {
            Value = stringResources.Values.ToDictionary((sr) => sr.Name, (sr) => sr.Value);
        }

        // Deserializes the XML file into a list of strings
        private StringResourceList DeserializeStrings(string path)
        {
            StringResourceList strings;

            string fullPath = $"{GetStringsDir()}/{GetLanguageCode()}/{path}";

            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(StringResourceList));
                    strings = (StringResourceList)deserializer.Deserialize(stream);
                }
            }
            else
            {
                throw new ArgumentException($"Unable to load strings xml from '{fullPath}.'  Please ensure the file exists.");
            }

            return strings;
        }

        /// <summary>
        /// Get the path of the 'strings' dir inside the resources folder
        /// </summary>
        protected abstract string GetStringsDir();

        /// <summary>
        /// Get the configured language code 
        /// </summary>
        protected abstract string GetLanguageCode();


    
    }
}

using Assets.Source.Configuration.Base;
using Assets.Source.Configuration.Constants;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Source.Configuration.Factory
{
    /// <summary>
    /// Use the ConfigFactory to load or save any serializable object as a configuration file.  These should be maintained via 
    /// the ConfigurationRepository to avoid unneccesary IO
    /// </summary>
    public class ConfigurationFactory
    {
        /// <summary>
        /// Loads the xml into an instance of the provided Type parameter.  If the file does not exist,
        /// Will return a defaulted instance of that object
        /// </summary>
        /// <typeparam name="T">Implementation of BaseConfiguration</typeparam>
        /// <returns>a casted instance of T</returns>
        public T LoadOrDefault<T>() where T : ConfigurationBase, new()
        {
            string fullFilePath = GenerateFileNameForT<T>();


            // Make sure the class is serializable
            if (typeof(T).GetCustomAttributes(typeof(SerializableAttribute), true).Length == 0)
            {
                throw new ArgumentException($"Type '{typeof(T).ToString()}' must be serializable");
            }

            // This actually instantiates the config with its defaulted values.
            T deserialized = new T();

            if (File.Exists(fullFilePath))
            {
                using (FileStream stream = new FileStream(fullFilePath, FileMode.Open))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(T));
                    deserialized = (T)deserializer.Deserialize(stream);
                }
            }

            return deserialized;
        }

        /// <summary>
        /// Save a serialized object into the directory that it belongs in. 
        /// </summary>
        /// <typeparam name="T">A BaseConfig Child Class</typeparam>
        public void Save<T>(T serializable) where T : ConfigurationBase, new()
        {
            string fullFilePath = GenerateFileNameForT<T>();

            CreateDirectoryIfNotExists(fullFilePath);

            using (FileStream stream = new FileStream(fullFilePath, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, serializable);
            }
        }

        /// <summary>
        /// Will create a directory if it does not exist...otherwise literally does nothin'
        /// </summary>
        /// <param name="dir">file directory to create</param>
        private void CreateDirectoryIfNotExists(string dir)
        {
            FileInfo fileInfo = new FileInfo(dir);
            fileInfo.Directory.Create();
        }

        /// <summary>
        /// Generates a file path in the configuration directory based on the class name
        /// </summary>
        private string GenerateFileNameForT<T>()
        {
            return $"{ConfigurationConstants.CONFIGURATION_DIRECTORY}/{typeof(T).Name}.xml";
        }
    }
}

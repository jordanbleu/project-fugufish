using System;
using System.Xml.Serialization;

namespace Assets.Source.Strings
{
    [Serializable]
    [XmlRoot(ElementName = "string")]
    public class StringResource
    {
        public StringResource() { }

        /// <summary>
        /// String resource identifier
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Value of the string 
        /// </summary>
        [XmlText]
        public string Value { get; set; }

    }
}

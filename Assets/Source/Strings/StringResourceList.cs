using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Source.Strings
{
    [XmlRoot("strings")]
    public class StringResourceList
    {
        public StringResourceList() { }

        [XmlElement("string")]
        public List<StringResource> Values { get; set; }
    }
}

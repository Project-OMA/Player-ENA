using System.Xml;

namespace ENA
{
    public static partial class XMLNodeExtensions
    {
        public static string GetValue(this XmlNode self, string key) => self.Attributes[key].Value;
    }
}
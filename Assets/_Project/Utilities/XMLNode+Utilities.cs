using System.Xml;
using UnityEngine;

namespace ENA
{
    public static partial class XMLNodeExtensions
    {
        public static string GetValue(this XmlNode self, string key)
        {
            return self.Attributes[key].Value;
        }
    }
}
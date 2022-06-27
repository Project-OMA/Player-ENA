using System.Xml;
using UnityEngine;

namespace ENA.Utilities
{
    public static partial class XMLNodeExtensions
    {
        public static string GetValue(this XmlNode self, string key)
        {
            return self.Attributes[key].Value;
        }
    }
}
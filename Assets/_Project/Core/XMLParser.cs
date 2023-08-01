using System.IO;
using System.Xml;

namespace ENA
{
    public class XMLParser
    {
        #region Variables
        XmlDocument document;
        #endregion
        #region Constructors
        public XMLParser(string xmlData)
        {
            document = DocumentFrom(xmlData);
        }
        #endregion
        #region Methods
        public XmlNode[] FetchAllItems(string path)
        {
            XmlNodeList list = document.SelectNodes(path);
            XmlNode[] array = new XmlNode[list.Count];

            for(int i = 0; i < list.Count; i++) {
                array[i] = list.Item(i);
            }

            return array;
        }

        public XmlNode Fetch(string path) => document.SelectSingleNode(path);
        #endregion
        #region Static Methods
        public static XMLParser Create(string xmlData) => new(xmlData);

        public static XmlDocument DocumentFrom(string xmlData)
        {
            xmlData = xmlData.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");
            var document = new XmlDocument();
            document.Load(new StringReader(xmlData));
            return document;
        }
        #endregion
    }
}
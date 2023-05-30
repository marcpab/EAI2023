using System.Xml.Linq;
using System.Xml;

namespace EAI.General.Xml.Extensions
{
    public static class XElementExtensions
    {
        public static XmlNode ToXmlNode(this XNode xNode)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xNode.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument.DocumentElement;
        }
    }
}

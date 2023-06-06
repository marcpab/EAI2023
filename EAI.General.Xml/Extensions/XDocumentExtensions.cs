using System.Xml.Linq;
using System.Xml;

namespace EAI.General.Xml.Extensions
{
    public static class XDocumentExtensions
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();

            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument;
        }
    }
}

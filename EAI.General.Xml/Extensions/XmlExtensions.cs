using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace EAI.General.Xml.Extensions
{
    public static class XmlExtensions
    {
        public static byte[] ToUTF8ByteArray(this XmlDocument document)
        {
            return Encoding.UTF8.GetBytes(document.OuterXml);
        }

        public static XmlElement CreateElement(this XmlDocument doc, XName xName, XAttribute xmlns)
        {
            if (xmlns == null)
                return doc.CreateElement(null, xName.LocalName, xName.NamespaceName);

            if (xName.NamespaceName == xmlns.Value)
                return doc.CreateElement(xmlns.Name.LocalName, xName.LocalName, xName.NamespaceName);

            var xmlElement = doc.CreateElement(null, xName.LocalName, xName.NamespaceName);
            xmlElement.SetAttribute(xmlns.Name.ToQualifiedName(), xmlns.Value);

            return xmlElement;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}

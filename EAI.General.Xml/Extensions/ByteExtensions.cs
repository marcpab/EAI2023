using System.IO;
using System.Xml;

namespace EAI.General.Xml.Extensions
{
    public static class ByteExtensions
    {
        public static XmlDocument ToXmlDocument(this byte[] byteXmlData)
        {
            var document = new XmlDocument();

            try
            {
                using (var memoryStream = new MemoryStream(byteXmlData))
                {
                    document.Load(memoryStream);
                }
            }
            catch
            {
                // ignore error and return empty xml
            }

            return document;
        }
    }
}

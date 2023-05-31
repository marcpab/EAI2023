using System.Xml.Linq;

namespace EAI.General.Xml.Extensions
{
    public static class XNameExtensions
    {
        public static string ToQualifiedName(this XName xName)
        {
            if (string.IsNullOrWhiteSpace(xName.NamespaceName))
                return xName.LocalName;

            return $"{xName.NamespaceName}:{xName.LocalName}";
        }
    }
}

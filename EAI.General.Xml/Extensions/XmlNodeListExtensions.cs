using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EAI.General.Xml.Extensions
{
    public static class XmlNodeListExtensions
    {
        public static IEnumerable<XmlNode> ToEnumerable(this XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
                yield return node;
        }
    }
}

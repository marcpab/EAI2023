using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Collections;
using System.Xml.Linq;

namespace EAI.General.Xml.Extensions
{
    public static class XmlNodeExtensions
    {
        public static dynamic ToDynamic(this XmlNode node, NodeDefaultBehavior missingNodeBehavior = NodeDefaultBehavior.Exception, Func<XmlNode, string, string> namespaceLookup = null, XmlSchema schema = null)
        {
            if (node == null)
                return null;

            var schemaElement = schema == null ? 
                null : 
                schema.Elements[new XmlQualifiedName(node.LocalName, node.NamespaceURI)] as XmlSchemaElement;

            return new DynamicXmlNode(node, schemaElement, missingNodeBehavior, namespaceLookup);
        }

        public static dynamic ToDynamic(this XmlNode node, NodeDefaultBehavior missingNodeBehavior = NodeDefaultBehavior.Exception)
        {
            if (node == null)
                return null;

            return new DynamicXmlNode(node, missingNodeBehavior);
        }

        public static XmlDocument ToXmlDocument(this XmlNode node)
        {
            if (node == null)
                return null;

            var document = new XmlDocument();
            document.AppendChild(
                document.ImportNode(node, true)
                );

            return document;
        }

        public static XElement ToXElement(this XmlNode node)
        {
            return XElement.Load(node.CreateNavigator().ReadSubtree());
        }

        public static XmlNode Create(this XmlNode node, string name, string namespaceUri = null)
        {
            string prefix = node.Prefix;
            string ns = node.NamespaceURI;
            bool nsProvided = false;

            if (!string.IsNullOrWhiteSpace(namespaceUri))
            {
                nsProvided = true;
                prefix = node.GetPrefixOfNamespace(namespaceUri);
                ns = namespaceUri;
            }

            if (Utilities.IsAttribute(name) && nsProvided)
            {
                return node.Attributes.Append(
                    node.OwnerDocument.CreateAttribute(
                        prefix,
                        Utilities.GetAttributeName(name),
                        ns)
                    );
            }

            if (Utilities.IsAttribute(name) && !nsProvided)
            {
                return node.Attributes.Append(
                    node.OwnerDocument.CreateAttribute(
                        Utilities.GetAttributeName(name))
                    );
            }

            return node.AppendChild(
                node.OwnerDocument.CreateNode(
                    XmlNodeType.Element,
                    prefix,
                    name,
                    ns)
                );
        }

        public static string GetValue(this XmlNode node)
            => node.GetValueOrDefault(null);

        public static string GetValueOrDefault(this XmlNode node, string defaultValue)
        {
            if (node == null)
                return defaultValue;

            return node.InnerText;
        }

        public static string GetValueOrDefault(this XmlNode node, string xPath, string defaultValue)
        {
            if (node == null)
                return defaultValue;

            return node.SelectSingleNode(xPath).GetValueOrDefault(defaultValue);
        }

        public static string GetValue(this XmlNode node, string xPath)
            => node.GetValueOrDefault(xPath, null);

        public static void RemoveNode(this XmlNode node, string xPath)
        {
            if (node == null)
                return;

            node.SelectSingleNode(xPath).RemoveNode();
        }

        public static void RemoveNode(this XmlNode node)
        {
            if (node == null)
                return;

            node.ParentNode.RemoveChild(node);
        }

        public static void ReplaceNode(this XmlNode node, XmlNode newNode)
        {
            if (node == null)
                return;

            if (newNode == null)
                return;

            if (!ReferenceEquals(node.OwnerDocument, newNode.OwnerDocument))
                newNode = node.OwnerDocument.ImportNode(newNode, true);
            else
                newNode = newNode.CloneNode(true);

            node.ParentNode.ReplaceChild(newNode, node);
        }

        public static void CopyFrom(this XmlNode node, XmlNode newNode, bool clearSourceNode = false)
        {
            if (node == null)
                return;

            if (newNode == null)
                return;

            if (!ReferenceEquals(node.OwnerDocument, newNode.OwnerDocument))
                newNode = node.OwnerDocument.ImportNode(newNode, true);
            else
                newNode = newNode.CloneNode(true);

            if (clearSourceNode)
            {
                node.Attributes.RemoveAll();
                node.RemoveAll();
            }

            foreach (XmlAttribute attribute in new ArrayList(newNode.Attributes))
                node.Attributes.Append(attribute);

            foreach (XmlNode childNode in new List<XmlNode>(newNode.ChildNodes.ToEnumerable()))
                node.AppendChild(childNode);
        }

        public static string GetMessageType(this XmlNode root)
        {
            if (string.IsNullOrEmpty(root.NamespaceURI))
                return root.LocalName;

            return $"{root.NamespaceURI}#{root.LocalName}";
        }

        public static void SetValue(this XmlNode node, string xPath, string value)
        {
            if (node == null)
                throw new ArgumentNullException($"Parameter node is null. XPath: {xPath}");

            var selectedNode = node.SelectSingleNode(xPath)
                ?? throw new InvalidOperationException($"Node not found. XPath: {xPath}");

            InternalSetValue(selectedNode, value);
        }

        public static void SetValue(this XmlNode node, string value)
        {
            if (node == null)
                throw new ArgumentNullException(null, "Parameter node is null.");

            InternalSetValue(node, value);
        }

        private static void InternalSetValue(XmlNode node, string value)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Attribute:
                    node.Value = value;
                    break;
                default:
                    node.InnerText = value;
                    break;
            }
        }

        public static XmlNode ChildOrCreate(this XmlNode node, string name)
        {
            var childNode = node.SelectSingleNode(Utilities.GetNodeXPath(name))
                ?? Create(node, name);

            return childNode;
        }


    }
}

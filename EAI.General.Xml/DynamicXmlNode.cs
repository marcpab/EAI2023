using System;
using System.Xml.Schema;
using System.Xml;
using EAI.General.Xml.Extensions;
using System.Collections;
using System.Dynamic;

namespace EAI.General.Xml
{
    public class DynamicXmlNode 
        : DynamicObject, IEnumerable
    {
        public bool SchemaTyping { get; private set; } = false;
        public XmlNode Current { get; private set; }
        public XmlNode Parent { get; private set; }
        public XmlSchemaElement SchemaElement { get; private set; }
        public XmlSchemaElement ParentSchemaElement { get; private set; }
        public string LocalName { get; private set; }
        public NodeDefaultBehavior NodeBehavior { get; private set; }
        private Func<XmlNode, string, string> NamespaceLookup { get; set; }


        public DynamicXmlNode(XmlNode xmlNode, NodeDefaultBehavior missingNodeBehavior) 
            : this(xmlNode, null, missingNodeBehavior, null)
        {
        }

        public DynamicXmlNode(XmlNode xmlNode, XmlSchemaElement schemaElement, NodeDefaultBehavior missingNodeBehavior, Func<XmlNode, string, string> namespaceLookup)
        {
            SchemaTyping = false;
            Current = xmlNode;
            SchemaElement = schemaElement;
            NodeBehavior = missingNodeBehavior;
            NamespaceLookup = namespaceLookup;
        }

        protected DynamicXmlNode(XmlNode parentXmlNode, XmlSchemaElement parentSchemaElement, string localName, NodeDefaultBehavior missingNodeBehavior, Func<XmlNode, string, string> namespaceLookup)
        {
            SchemaTyping = false;
            Parent = parentXmlNode;
            ParentSchemaElement = parentSchemaElement;
            LocalName = localName;
            NodeBehavior = missingNodeBehavior;
            NamespaceLookup = namespaceLookup;
        }

        static public implicit operator string(DynamicXmlNode node)
        {
            return node?.Value;
        }

        static public implicit operator XmlNode(DynamicXmlNode node)
        {
            return node?.GetCurrentNode();
        }

        public IEnumerator GetEnumerator()
        {
            var currentSchemaElement = GetCurrentSchemaElement();

            if (Parent == null)
                yield break;

            foreach (XmlNode node in Parent.SelectNodes($"./*[local-name() = '{LocalName}']"))
                yield return new DynamicXmlNode(node, currentSchemaElement, NodeBehavior, NamespaceLookup);
        }

        public string Value
        {
            get
            {
                Current = GetCurrentNode();
                return Current?.GetValue();
            }
            set
            {
                Current = GetCurrentNode();
                Current.InnerText = value;
            }
        }

        public dynamic GetNode(string name)
        {
            var currentXmlNode = GetCurrentNode();
            var currentSchemaElement = GetCurrentSchemaElement();

            return new DynamicXmlNode(currentXmlNode, currentSchemaElement, name, NodeBehavior, NamespaceLookup);
        }

        public bool SetNode(string name, object value)
        {
            var currentXmlNode = GetCurrentNode();

            if (value == null)
            {
                currentXmlNode.SelectSingleNode(Utilities.GetNodeXPath(name)).RemoveNode();
                return true;
            }

            var newChildXmlNode = currentXmlNode.SelectSingleNode(Utilities.GetNodeXPath(name));
            if (newChildXmlNode == null)
            {
                var currentSchemaElement = GetCurrentSchemaElement();
                newChildXmlNode = CreateNode(currentXmlNode, currentSchemaElement, name);
            }

            return SetValue(newChildXmlNode, value);
        }

        private static bool SetValue(XmlNode valueNode, object value)
        {
            if (value == null)
                return false;

            var valueIsXmlNode = value as XmlNode;
            
            // when we didnt get a XmlNode, we try to get a DynamicXml
            if (valueIsXmlNode == null)
            {
                var valueIsDynamicNode = value as DynamicXmlNode;
                if (valueIsDynamicNode != null)
                    valueIsXmlNode = valueIsDynamicNode.GetCurrentNode();
            }

            // when we didnt get a XmlNode or DynamicXml, we assume we got
            // an object that posesses ToString method
            if (valueIsXmlNode == null)
            {
                var valueIsString = value.ToString();
                if (valueIsString == null)
                    return false;

                valueNode.InnerText = valueIsString;
                return true;
            }

            valueNode.CopyFrom(valueIsXmlNode, true);

            return true;
        }
        
        private XmlNode GetCurrentNode(object[] indexes = null)
        {
            if (Current != null)
                return Current;

            if (Parent == null)
                return null;

            // createNode Logic and xPath depends on parameter indexes

            // default behavior:
            string xPath = $"./*[local-name() = '{LocalName}']";
            Func<XmlNode> createNode = () =>
                {
                    var currentNode = Parent.SelectSingleNode(xPath);
                    if (currentNode != null)
                        return currentNode;

                    return CreateNode(Parent, ParentSchemaElement, LocalName);
                };
            
            if (indexes.Length == 1 && indexes[0].GetType().IsValueType)
            {
                int index = (int)indexes[0];
                xPath = null;

                createNode = () 
                        => CreateNode(Parent, ParentSchemaElement, LocalName);
                
                if (index != -1)
                {
                    if (index < 1)
                        throw new InvalidOperationException(string.Format("XPath index is 1 based. Current index {0}", index));

                    xPath = $"./*[local-name() = '{LocalName}'][{index}]";

                    createNode = () 
                        =>
                    {
                        XmlNode newXmlNode = null;
                        while ((newXmlNode = Parent.SelectSingleNode(xPath)) == null)
                            CreateNode(Parent, ParentSchemaElement, LocalName);

                        return newXmlNode;
                    };
                }
            }
            else if (indexes.Length == 2)
            {
                var keyXmlNodeName = indexes[0].ToString();

                xPath = string.Format(
                        "./*[local-name() = '{0}' and ./*[local-name() = '{1}' and . = '{2}']]",
                        LocalName,
                        keyXmlNodeName,
                        indexes[1]);

                if (Utilities.IsAttribute(keyXmlNodeName))
                {
                    xPath = string.Format(
                        "./*[local-name() = '{0}' and ./@*[local-name() = '{1}' and . = '{2}']]",
                        LocalName,
                        Utilities.GetAttributeName(keyXmlNodeName),
                        indexes[1]);
                }

                createNode = () =>
                {
                    var newChildXmlNode = CreateNode(Parent, ParentSchemaElement, LocalName);
                    var newChildSchemaElement = GetCurrentSchemaElement();

                    CreateNode(newChildXmlNode, 
                        newChildSchemaElement.GetChildSchemaElement(keyXmlNodeName), 
                        keyXmlNodeName)
                            .SetValue(indexes[1].ToString());

                    return newChildXmlNode;
                };
            }

            var childXmlNode = string.IsNullOrWhiteSpace(xPath) ? 
                null : 
                Parent.SelectSingleNode(xPath);

            if (childXmlNode != null)
            {
                if (NodeBehavior == NodeDefaultBehavior.EmptyToNull &&
                   string.IsNullOrWhiteSpace(childXmlNode.Value) &&
                   !childXmlNode.HasChildNodes)
                    return null;
                else
                    return childXmlNode;
            }

            switch (NodeBehavior)
            {
                case NodeDefaultBehavior.EmptyToNull:
                case NodeDefaultBehavior.Null:
                    return null;
                case NodeDefaultBehavior.Exception:
                    throw new InvalidOperationException($"Could not locate node {LocalName} (xPath: {xPath ?? "[NULL]"}).");
                case NodeDefaultBehavior.CreateFromSchema:
                    SchemaTyping = true;
                    return createNode();
                case NodeDefaultBehavior.Create:
                    return createNode();
                default:
                    throw new InvalidOperationException($"MissingNodeBehavior {NodeBehavior} not implemented!");
            }
        }

        private XmlNode CreateNode(XmlNode parent, XmlSchemaElement parentSchemaElement, string localName)
        {
            // when SchemaValidation is required, then schemaElement must be present
            var schemaElement = parentSchemaElement.GetChildSchemaElement(localName);
            if (SchemaTyping && schemaElement == null)
                throw new InvalidOperationException($"{localName} not found in schema");

            var newNode = parent.Create(
                localName,
                GetNamespace(parent, schemaElement, localName)
                );

            foreach (var followingSchemaElement in parentSchemaElement.GetChildSchemaElements(localName))
            {
                var nextNode = Parent.SelectSingleNode(Utilities.GetNodeXPath(followingSchemaElement.QualifiedName.Name));
                if (nextNode != null)
                {
                    newNode.RemoveNode();
                    Parent.InsertBefore(newNode, nextNode);
                    break;
                }
            }

            return newNode;
        }

        private XmlSchemaElement GetCurrentSchemaElement()
        {
            SchemaElement = ParentSchemaElement.GetChildSchemaElement(LocalName);
            return SchemaElement;
        }

        private string GetNamespace(XmlNode parent, XmlSchemaElement schemaElement, string localName)
        {
            var namespaceUri = NamespaceLookup != null 
                                    ? NamespaceLookup(parent, localName) 
                                    : null;

            if (namespaceUri != null)
                return namespaceUri;

            if (Utilities.IsAttribute(localName))
                return string.Empty;

            if (schemaElement != null)
                return schemaElement.QualifiedName.Namespace;

            return parent.NamespaceURI;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetNode(binder.Name);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return SetNode(binder.Name, value);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var currentXmlNode = GetCurrentNode(indexes);
            var currentSchemaElement = GetCurrentSchemaElement();

            result = currentXmlNode == null ?
                new DynamicXmlNode(null, null, null, NodeBehavior,NamespaceLookup) :
                new DynamicXmlNode(currentXmlNode, currentSchemaElement, NodeBehavior, NamespaceLookup);

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var currentXmlNode = GetCurrentNode(indexes);

            if (value == null)
            {
                currentXmlNode.RemoveNode();
                return true;
            }

            return SetValue(currentXmlNode, value);
        }
    }
}

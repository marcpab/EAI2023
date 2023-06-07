using EAI.General.Xml.Extensions;
using System;
using System.Dynamic;
using System.Xml;

namespace EAI.General.Xml
{
    public class DynamicXmlAttributes 
        : DynamicObject
    {
        private XmlNode Node { get; set; }
        private NodeDefaultBehavior NodeBehavior { get; set; }

        public DynamicXmlAttributes(XmlNode xmlNode, NodeDefaultBehavior nodeBehavior)
        {
            Node = xmlNode;
            NodeBehavior = nodeBehavior;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetAttribute(binder.Name).GetValue();

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (value == null)
                GetAttribute(binder.Name).RemoveNode();
            else
                GetAttribute(binder.Name).SetValue(value.ToString());

            return true;
        }

        private XmlNode GetAttribute(string name)
        {
            if (Node == null)
                return null;

            var attributeName = Utilities.GetAsAttributeName(name);
            var nodeXPath = Utilities.GetNodeXPath(attributeName);

            var xmlAttribute = Node.SelectSingleNode(nodeXPath);
            if (xmlAttribute != null)
            {
                if (NodeBehavior == NodeDefaultBehavior.EmptyToNull &&
                    string.IsNullOrWhiteSpace(xmlAttribute.Value))
                    return null;
                else
                    return xmlAttribute;
            }

            // when there is nothing, than the behavior decides...
            switch (NodeBehavior)
            {
                case NodeDefaultBehavior.EmptyToNull:
                case NodeDefaultBehavior.Default:
                    return null;
                case NodeDefaultBehavior.Exception:
                    throw new InvalidOperationException($"Could not find attribute {name}");
                case NodeDefaultBehavior.CreateFromSchema:
                case NodeDefaultBehavior.Create:
                    return Node.Create(attributeName);
                default:
                    throw new InvalidOperationException($"Invalid MissingNodeBehavior {NodeBehavior}");
            }
        }
    }
}

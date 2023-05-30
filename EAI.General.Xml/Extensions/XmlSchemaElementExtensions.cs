using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace EAI.General.Xml.Extensions
{
    public static class XmlSchemaElementExtensions
    {
        public static XmlSchemaElement GetChildSchemaElement(this XmlSchemaElement schemaElement, string childLocalName)
        {
            if (schemaElement == null)
                return null;

            return schemaElement
                .GetChildSchemaElements()
                .Where(x => x.QualifiedName.Name == childLocalName)
                .FirstOrDefault();
        }

        public static IEnumerable<XmlSchemaElement> GetChildSchemaElements(this XmlSchemaElement schemaElement)
        {
            if (schemaElement == null)
                yield break;

            var workItems = new Stack<object>();
            workItems.Push(schemaElement.ElementSchemaType as XmlSchemaComplexType);

            while (workItems.Count > 0)
            {
                var item = workItems.Pop();
                if (item == null)
                    continue;

                var complexType = item as XmlSchemaComplexType;
                if (complexType != null)
                {
                    workItems.Push(complexType.ContentTypeParticle);

                    continue;
                }

                var element = item as XmlSchemaElement;
                if (element != null)
                {
                    yield return element;

                    continue;
                }

                var groupBase = item as XmlSchemaGroupBase;
                if (groupBase != null)
                {
                    foreach (var groupItem in groupBase.Items.Cast<object>().Reverse())
                        workItems.Push(groupItem);

                    continue;
                }

                var groupRef = item as XmlSchemaGroupRef;
                if (groupRef != null)
                {
                    workItems.Push(groupRef.Particle);

                    continue;
                }
            }
        }

        public static IEnumerable<XmlSchemaElement> GetChildSchemaElements(this XmlSchemaElement schemaElement, string childLocalName)
        {
            var found = false;
            foreach (var element in schemaElement.GetChildSchemaElements())
            {
                if (element != null && found)
                {
                    yield return element;
                    continue;
                }

                if (element != null && element.QualifiedName.Name == childLocalName)
                    found = true;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace EAI.General.Xml.Extensions
{
    public static class XmlSchemaElementExtensions
    {
        public static XmlSchemaElement GetChildSchemaElement(this XmlSchemaElement schemaElement, string childLocalName)
        {
            if (schemaElement is null)
                return null;

            return schemaElement
                .GetChildSchemaElements()
                .Where(x => x.QualifiedName.Name == childLocalName)
                .FirstOrDefault();
        }

        public static IEnumerable<XmlSchemaElement> GetChildSchemaElements(this XmlSchemaElement schemaElement)
        {
            if (schemaElement is null)
                yield break;

            var workItems = new Stack<object>();
            workItems.Push(schemaElement.ElementSchemaType as XmlSchemaComplexType);

            while (workItems.Count > 0)
            {
                var item = workItems.Pop();
                if (item is null)
                    continue;

                if (item is XmlSchemaComplexType complexType)
                {
                    workItems.Push(complexType.ContentTypeParticle);

                    continue;
                }

                if (item is XmlSchemaElement element)
                {
                    yield return element;

                    continue;
                }

                if (item is XmlSchemaGroupBase groupBase)
                {
                    foreach (var groupItem in groupBase.Items.Cast<object>().Reverse())
                        workItems.Push(groupItem);

                    continue;
                }

                if (item is XmlSchemaGroupRef groupRef)
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

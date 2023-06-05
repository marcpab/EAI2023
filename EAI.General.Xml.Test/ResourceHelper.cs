using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EAI.General.Xml.Test
{
    internal static class ResourceHelper
    {
        private static readonly string XmlFolder = "XmlTestDocs";

        private static readonly string Xml_ChemicalTest1 = "ZDebmas07_SampleChemicalIndustry01.xml";

        internal static XDocument? XmlChemicalTest01
        {
            get
            {
                var str = GetXml(Xml_ChemicalTest1);
                if (string.IsNullOrWhiteSpace(str))
                    return null;

                return XDocument.Parse(str);
            }
        }

        internal static string? GetXml(string name) => GetString($"EAI.General.Xml.Test.{XmlFolder}.{name}");

        private static string? GetString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream? stream = assembly?.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();

                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace EAI.General.Xml.Test
{
    internal static class ResourceHelper
    {
        private static readonly string XmlFolder = "XmlTestDocs";

        private static readonly string Xml_ChemicalTest1 = "ZDebmas07_SampleChemicalIndustry01.xml";

        internal static XDocument? ZDebmas07_SampleChemicalIndustry01
        {
            get
            {
                return GetXDoc(Xml_ChemicalTest1);
            }
        }

        internal static XmlDocument? Xml_ZDebmas07_SampleChemicalIndustry01
        {
            get
            {
                return GetXmlDoc(Xml_ChemicalTest1);
            }
        }

        private static XmlDocument? GetXmlDoc(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"EAI.General.Xml.Test.{XmlFolder}.{name}";

            using (Stream? stream = assembly?.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (XmlReader reader = XmlReader.Create(stream))
                {
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    return doc;
                }
            }
        }

        private static XDocument? GetXDoc(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"EAI.General.Xml.Test.{XmlFolder}.{name}";

            using (Stream? stream = assembly?.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                return XDocument.Load(stream);
            }
        }

        public static string? GetString(string name) 
            => Read($"EAI.General.Xml.Test.{XmlFolder}.{name}");

        private static string? Read(string resourceName)
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

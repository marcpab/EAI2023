using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace EAI.General
{
    public class EmbeddedResources
    {
        public static string GetResourceString(string resourceName, Encoding encoding)
        {
            using (var stream = GetResourceStream(Assembly.GetCallingAssembly(), resourceName))
            using (var reader = new StreamReader(stream, encoding))
                return reader.ReadToEnd();

        }

        public static T GetResource<T>(string resourceName, Encoding encoding)
        {
            using (var stream = GetResourceStream(Assembly.GetCallingAssembly(), resourceName))
            using (var reader = new StreamReader(stream, encoding))
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        public static Stream GetResourceStream(string resourceName)
        {
            return GetResourceStream(Assembly.GetCallingAssembly(), resourceName);
        }

        private static Stream GetResourceStream(Assembly resourceAssembly, string resourceName)
        {
            var assemblyName = new AssemblyName(resourceAssembly.FullName);


            var fullResourceName = $"{assemblyName.Name}.Resources.{resourceName}";

            var resource = resourceAssembly.GetManifestResourceStream(fullResourceName);
            if(resource != null)
                return resource;

            var resources = resourceAssembly.GetManifestResourceNames();

            throw new EAIException($"Resource {fullResourceName} not found. Available resources {string.Join(", ", resources)}");
        }
    }
}

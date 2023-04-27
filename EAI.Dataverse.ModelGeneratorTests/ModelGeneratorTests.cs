using Xunit;
using EAI.Dataverse.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using EAI.Rest;
using Newtonsoft.Json;
using EAI.OData;
using EAI.Dataverse.ModelGeneratorTests.Model;
using System.Threading.Tasks;
using System.IO;

namespace EAI.Dataverse.ModelGenerator.Tests
{
    public class ModelGeneratorTests
    {
        [Fact()]
        public async Task GenerateCodeTest()
        {
            var setting = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            var configJson = GetConfigResource();

            var odataClient = JsonConvert.DeserializeObject<ODataClient>(configJson, setting);
            var modelGenerator = new ModelGenerator()
            {
                GenerateDynamicNavigationProperties = true,
                GenerateLookups = true,
                GenerateNavigationProperties = true,
                UseEnumTypeForPicklistProperties = true
            };

            var code = await modelGenerator.GenerateCodeAsync(odataClient, typeof(account).Assembly);

            ModelWriter.Write(typeof(account).Assembly, code);
        }

        private static string GetConfigResource()
        {
            using (var stream = typeof(ModelGeneratorTests).Assembly.GetManifestResourceStream("EAI.Dataverse.ModelGeneratorTests.config.json"))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
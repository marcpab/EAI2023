using EAI.Dataverse.ModelGenerator.Tokens;
using EAI.OData;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.Dataverse.ModelGenerator
{
    public class ModelGenerator
    {
        public bool UseEnumTypeForPicklistProperties { get; set; }
        public bool GenerateLookups { get; set; } = false;
        public bool GenerateNavigationProperties { get; set; } = false;
        public bool GenerateDynamicNavigationProperties { get; set; } = false;

        public async Task<string> GenerateCodeAsync(ODataClient cdsclient, Assembly modelAssembly, string oDataV4MetadataPath = null)
        {
            var tokenList = new List<IToken>();

            var view = new ModelAssemblyView() { ModelAssembly = modelAssembly };


            var odataTokens = new ODataTokens()
            {
                UseEnumTypeForPicklistProperties = UseEnumTypeForPicklistProperties,
                GenerateLookups = GenerateLookups,
                GenerateNavigationProperties = GenerateNavigationProperties,
                GenerateDynamicNavigationProperties = GenerateDynamicNavigationProperties,
            };

            tokenList.Add(new UsingToken { Namespace = "System" });
            tokenList.Add(new UsingToken { Namespace = "EAI.OData" });

            tokenList.AddRange(await odataTokens.GetEntityTokensAsync(cdsclient, view));

            if (!string.IsNullOrEmpty(oDataV4MetadataPath))
            {

                var meatadataTokens = new MetadataTokens()
                {
                    //UseEnumTypeForPicklistProperties = UseEnumTypeForPicklistProperties,
                    //GenerateLookups = GenerateLookups,
                    //GenerateNavigationProperties = GenerateNavigationProperties,
                    //GenerateDynamicNavigationProperties = GenerateDynamicNavigationProperties,
                };

                tokenList.AddRange(await meatadataTokens.GetMeatdataTokensAsync(oDataV4MetadataPath, view));
            }

            return CreateCode(tokenList);
        }

        protected static string CreateCode(List<IToken> tokenList)
        {
            var code = new StringBuilder();

            foreach (var token in tokenList)
                token.Write(code);

            return code.ToString();
        }
    }
}
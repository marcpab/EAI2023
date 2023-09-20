using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class ExecuteToken : IToken
    {
        public string CSharpType { get; set; }
        public string Entity { get; set; }

        public void Write(StringBuilder _code)
        {

            _code.AppendLine("");
            _code.AppendLine($"\t\tpublic Task<{CSharpType}> ExecuteAsync(ODataClient odataClient)");
            _code.AppendLine($"\t\t\t=> odataClient.Create(nameof({Entity}), this).AsSingleAsync<{CSharpType}>();");
        }

        //public static Task<cdm_company> GetByNameAsync(ODataClient odataClient, string name)
        //           => odataClient

        // await client.Create("msdyn_CreateProjectV1", createProjectV1Request).AsSingleAsync<msdyn_CreateProjectV1Response>()
    }
}

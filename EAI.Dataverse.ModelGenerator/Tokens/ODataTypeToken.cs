using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class ODataTypeToken : IToken
    {
        public string EntityName { get; set; }

        public void Write(StringBuilder code)
        {

            code.AppendLine($"\t\tpublic ODataType ODataType {{ get => new ODataType() {{ Name = \"Microsoft.Dynamics.CRM.{EntityName}\" }}; }}");
            code.AppendLine();
        }
    }
}

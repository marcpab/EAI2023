using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class StatusCodeToken : IToken
    {
        //public string ODataName { get; set; }

        //public string DisplayName { get; set; }
        //public string Description { get; set; }
        //public string AttributeOf { get; set; }
        //public string AttributeType { get; set; }
        //public int MaxLength { get; set; }

        //public string CSharpAttribute { get; set; }
        //public string CSharpType { get; set; }
        public bool UseEnumTypeForPicklistProperties { get; set; }

        public void Write(StringBuilder code)
        {
            var cSharpType = UseEnumTypeForPicklistProperties ? "statuscodeEnum?" : "int?";

            code.AppendLine($"\t\tpublic override int? GetStatusCode() {{ return (int?)statuscode; }}");
            code.AppendLine($"\t\tpublic override void SetStatusCode(int? value) {{ statuscode = (statuscodeEnum?)value; }}");
            code.AppendLine();
        }
    }
}

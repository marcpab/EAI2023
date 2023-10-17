using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class StateCodeToken : IToken
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
            var cSharpType = UseEnumTypeForPicklistProperties ? "statecodeEnum?" : "int?";

            code.AppendLine($"\t\tpublic override int? GetStateCode() {{ return (int?)statecode; }}");
            code.AppendLine($"\t\tpublic override void SetStateCode(int? value) {{ statecode = ({cSharpType})value; }}");
            code.AppendLine();
        }
    }
}

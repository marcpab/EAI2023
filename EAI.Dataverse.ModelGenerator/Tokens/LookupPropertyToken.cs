using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class LookupPropertyToken : IToken
    {
        public string ODataName { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AttributeType { get; set; }
        public IEnumerable<string> Targets { get; set; }
        public string CSharpType { get; set; }

        public void Write(StringBuilder code)
        {
            code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(DisplayName))
                code.AppendLine($"\t\t/// Display name: {DisplayName}");
            if (!string.IsNullOrEmpty(Description))
                code.AppendLine($"\t\t/// Description : {Description}");
            code.AppendLine($"\t\t/// {AttributeType}, targets: {string.Join(", ", Targets)}");
            code.AppendLine($"\t\t/// </summary>");

            code.AppendLine($"\t\tpublic {CSharpType} {Utils.ExcapeName(ODataName)} {{ get; set; }}");  // {a.AttributeType}, targets: {string.Join(", ", a.Targets)}
        }
    }
}

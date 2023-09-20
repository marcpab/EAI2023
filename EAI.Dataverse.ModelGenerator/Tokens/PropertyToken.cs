using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class PropertyToken : IToken
    {
        public string ODataName { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AttributeOf { get; set; }
        public string AttributeType { get; set; }
        public int MaxLength { get; set; }
        public string CSharpType { get; set; }

        public void Write(StringBuilder _code)
        {
            _code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(DisplayName))
                _code.AppendLine($"\t\t/// Display name: {Utils.MultilineComment(DisplayName)}");
            if (!string.IsNullOrEmpty(Description))
                _code.AppendLine($"\t\t/// Description : {Utils.MultilineComment(Description)}");
            if (!string.IsNullOrEmpty(AttributeOf))
                _code.AppendLine($"\t\t/// Attribute of: {AttributeOf}");
            _code.AppendLine($"\t\t/// {AttributeType}, length {MaxLength}");
            _code.AppendLine($"\t\t/// </summary>");

            _code.AppendLine($"\t\tpublic {CSharpType} {Utils.ExcapeName(ODataName)};");
        }
    }
}

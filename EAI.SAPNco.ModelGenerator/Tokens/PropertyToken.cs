using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    class PropertyToken : IToken
    {
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public string CSharpType { get; set; }

        public void Write(StringBuilder _code)
        {
            _code.AppendLine($"");
            _code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(Description))
                _code.AppendLine($"\t\t/// Description: {Utils.MultilineComment(Description)}");
            _code.AppendLine($"\t\t/// </summary>");

            _code.AppendLine($"\t\tpublic {CSharpType} {Utils.EscapeName(PropertyName)} {{ get; set; }}");
        }
    }
}

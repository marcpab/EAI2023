using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    class RfcParameterToken : IToken
    {
        public string ParameterName { get; set; }
        public string Documentation { get; set; }
        public string Direction { get; set; }
        public string DataType { get; set; }
        public string AbapType { get; set; }
        public int NuLength { get; set; }
        public int Decimals { get; set; }
        public bool Optional { get; set; }
        public string DefaultValue { get; set; }
        public string CSharpType { get; set; }

        public void Write(StringBuilder _code)
        {
            _code.AppendLine($"");
            _code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(Documentation))
                _code.AppendLine($"\t\t/// Documentation: {Utils.MultilineComment(Documentation)}");
            if (!string.IsNullOrEmpty(Direction))
                _code.AppendLine($"\t\t/// Direction    : {Direction}");
            _code.AppendLine($"\t\t/// Type         : {DataType} ({AbapType}), length {NuLength}, decimals {Decimals}");
            _code.AppendLine($"\t\t/// Optional     : {Optional}");
            _code.AppendLine($"\t\t/// DefaultValue : {DefaultValue}");
            _code.AppendLine($"\t\t/// </summary>");

            _code.AppendLine($"\t\tpublic {CSharpType} {Utils.EscapeName(ParameterName)} {{ get; set; }}");
        }
    }
}

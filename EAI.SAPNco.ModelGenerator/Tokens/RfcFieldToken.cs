using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    class RfcFieldToken : IToken
    {
        public string FieldName { get; set; }
        public string Documentation { get; set; }
        public string DataType { get; set; }
        public string AbapType { get; set; }
        public int NuLength { get; set; }
        public int Decimals { get; set; }
        public string CSharpType { get; set; }

        public void Write(StringBuilder _code)
        {
            _code.AppendLine($"");
            _code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(Documentation))
                _code.AppendLine($"\t\t/// Documentation: {Utils.MultilineComment(Documentation)}");
            _code.AppendLine($"\t\t/// Type         : {DataType} ({AbapType}), length {NuLength}, decimals {Decimals}");
            _code.AppendLine($"\t\t/// </summary>");

            _code.AppendLine($"\t\tpublic {CSharpType} {Utils.EscapeName(FieldName)} {{ get; set; }}");
        }
    }
}

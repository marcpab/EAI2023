using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    class RfcFunctionToken : IToken, ITokenName
    {
        public string Namespace { get; set; }

        public string Description { get; set; }
        public string RfcFunctionName { get; set; }

        public IEnumerable<IToken> ChildTokens { get; set; }

        public string Name { get => RfcFunctionName; set => RfcFunctionName = value; }

        public void Write(StringBuilder code)
        {
            WriteStart(code);

            if(ChildTokens!= null)
                foreach (var token in ChildTokens)
                    token.Write(code);

            WriteEnd(code);
        }

        private void WriteStart(StringBuilder _code)
        {
            _code.AppendLine($"");
            _code.AppendLine($"namespace {Namespace}");
            _code.AppendLine("{");
            _code.AppendLine("\tusing System;");
            _code.AppendLine("\tusing System.Collections.Generic;");
            _code.AppendLine($"");
            _code.AppendLine($"\t/// <summary>");
            if (!string.IsNullOrEmpty(Description))
                _code.AppendLine($"\t/// Description : {Utils.MultilineComment(Description)}");
            _code.AppendLine($"\t/// </summary>");
            _code.AppendLine($"\tpublic partial class {Utils.ExcapeName(RfcFunctionName)}");

            _code.AppendLine("\t{");
        }

        private void WriteEnd(StringBuilder _code)
        {
            _code.AppendLine("\t}");
            _code.AppendLine("}");
            _code.AppendLine("");
        }

    }

}

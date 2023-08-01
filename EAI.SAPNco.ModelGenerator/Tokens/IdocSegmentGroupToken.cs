using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    internal class IdocSegmentGroupToken : IToken, ITokenFileName
    {
        public string Namespace { get; set; }
        public string SegmentName { get; set; }

        public IEnumerable<IToken> ChildTokens { get; set; }

        public string Name { get => $"{SegmentName}GRP"; }
        public string Folder { get => "_segment"; }

        public void Write(StringBuilder code)
        {
            WriteStart(code);

            if (ChildTokens != null)
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
            _code.AppendLine($"\t/// idoc segment group : {SegmentName}");
            _code.AppendLine($"\t/// </summary>");
            _code.AppendLine($"\tpublic partial class {Utils.Codeify(SegmentName)}GRP");
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

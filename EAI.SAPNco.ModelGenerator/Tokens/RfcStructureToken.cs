using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator.Tokens
{
    class RfcStructureToken : IToken, ITokenFileName
    {
        public string Namespace { get; set; }
        public string StructureName { get; set; }

        public IEnumerable<IToken> ChildTokens { get; set; }

        public string Name { get => StructureName; }
        public string Folder { get => "_structure"; }

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
            _code.AppendLine($"\t/// Rfc structure : {StructureName}");
            _code.AppendLine($"\t/// </summary>");
            _code.AppendLine($"\tpublic partial class {Utils.Codeify(StructureName)}");
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

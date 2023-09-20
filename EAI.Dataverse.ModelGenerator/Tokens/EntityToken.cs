using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class EntityToken : IToken, ITokenFileName
    {
        public string Namespace { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Entity { get; set; }

        public IEnumerable<IToken> ChildTokens { get; set; }

        public string Folder => null;

        public string Name => Utils.ExcapeName(Entity);

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
            _code.AppendLine($"namespace {Namespace}");
            _code.AppendLine("{");
            _code.AppendLine("\tusing System;");
            _code.AppendLine("\tusing EAI.OData;");
            _code.AppendLine($"");
            _code.AppendLine($"\t/// <summary>");
            if (!string.IsNullOrEmpty(DisplayName))
                _code.AppendLine($"\t/// Display name: {Utils.MultilineComment(DisplayName)}");
            if (!string.IsNullOrEmpty(Description))
                _code.AppendLine($"\t/// Description : {Utils.MultilineComment(Description)}");
            _code.AppendLine($"\t/// </summary>");
            _code.AppendLine($"\tpublic partial class {Utils.ExcapeName(Entity)}");

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

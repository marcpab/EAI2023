using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class UsingToken : IToken
    {
        public string Namespace { get; set; }

        public void Write(StringBuilder code)
        {
            code.AppendLine($"using {Namespace};");
        }
    }
}

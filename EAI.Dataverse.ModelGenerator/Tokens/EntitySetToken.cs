using EAI.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class EntitySetToken : IToken
    {
        public string EntitySetName { get; set; }

        public void Write(StringBuilder code)
        {
            code.AppendLine($"\t\tpublic const string EntitySet = \"{EntitySetName}\";");
            code.AppendLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator
{
    public interface IToken
    {
        void Write(StringBuilder code);
    }
}

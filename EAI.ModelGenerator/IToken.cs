using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.ModelGenerator
{
    public interface IToken
    {
        void Write(StringBuilder code);
    }
}

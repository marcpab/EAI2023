using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator
{
    public interface IToken
    {
        void Write(StringBuilder code);
    }
}

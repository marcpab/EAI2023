using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.ModelGenerator
{
    public interface ITokenFileName
    {
        string Folder { get; }
        string Name { get; }
    }
}

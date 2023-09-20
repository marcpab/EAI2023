using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.ModelGenerator
{
    public interface ITokenFileName
    {
        string Folder { get; }
        string Name { get; }
    }
}

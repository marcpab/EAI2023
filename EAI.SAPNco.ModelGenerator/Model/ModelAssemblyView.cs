using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EAI.Dataverse.ModelGenerator
{
    internal class ModelAssemblyView
    {
        private Assembly _modelAssembly;

        public IEnumerable<Type> Types { get => _modelAssembly
                                    .GetExportedTypes(); 
        }

        public Assembly ModelAssembly { get => _modelAssembly; set => _modelAssembly = value; }
    }
}

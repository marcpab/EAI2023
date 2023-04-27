using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EAI.Dataverse.ModelGenerator
{
    internal class ModelAssemblyView
    {
        private static Type _baseEntityType = typeof(DataverseEntity);
        private static Type _baseActionType = typeof(DataverseAction);

        private Assembly _modelAssembly;

        public Type BaseEntityType { get => _baseEntityType; }

        public Type BaseActionType { get => _baseActionType; }

        public IEnumerable<Type> EntityTypes { get => _modelAssembly
                                    .GetTypes()
                                    .Where(t => t.BaseType == _baseEntityType); 
        }

        public IEnumerable<string> BaseEntityProperties { get => _baseEntityType
                                    .GetProperties()
                                    .Select(p => p.Name); 
        }

        public IEnumerable<Type> ActionTypes { get => _modelAssembly
                                    .GetTypes()
                                    .Where(t => t.BaseType == _baseActionType);
        }

        public Assembly ModelAssembly { get => _modelAssembly; set => _modelAssembly = value; }
    }
}

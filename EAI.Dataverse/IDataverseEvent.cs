using EAI.OData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.Dataverse
{
    public interface IDataverseEvent
    {
        string Action { get; set; }
        string PrimaryEntityName { get; set; }

    }
}

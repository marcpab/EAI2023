using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public class ODataType
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"@odata.type:{Name}";
        }
    }
}

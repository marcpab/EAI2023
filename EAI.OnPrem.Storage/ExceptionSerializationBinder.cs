using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OnPrem.Storage
{
    public class ExceptionSerializationBinder : DefaultSerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                return base.BindToType(assemblyName, typeName);
            }
            catch(JsonSerializationException ex)
            {
                if(typeName.ToLower().Contains("exception"))
                    return typeof(Exception);
                
                throw;
            }
        }
    }
}

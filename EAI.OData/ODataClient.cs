using EAI.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public class ODataClient : RestClient
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new ODataContractResolver()
        };

        public ODataClient() 
        { 
            SerializerSettings = _serializerSettings;
        }
    }
}

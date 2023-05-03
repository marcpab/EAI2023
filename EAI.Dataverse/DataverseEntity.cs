using EAI.OData;
using Newtonsoft.Json;
using System;

namespace EAI.Dataverse
{
    public class DataverseEntity
    {
        [JsonProperty("@odata.etag")]
        public string ODataEtag { get; set; }
        public ODataType ODataType { get; set; }

        public int? statecode { get; set; }  // State
        public string statecodename { get; set; }  // Virtual
        public int? statuscode { get; set; }  // Status
        public string statuscodename { get; set; }  // Virtual

        public void SetActive()
        {
            statuscode = 1;
            statecode = 0;
        }

        public void SetInactive()
        {
            statuscode = 2;
            statecode = 1;
        }

        [JsonIgnore]
        public bool IsActive { get => statuscode == 1 && statecode == 0; }
    }
}

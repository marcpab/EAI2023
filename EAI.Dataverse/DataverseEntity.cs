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

        public void SetActive()
        {
            SetStatusCode(1);
            SetStateCode(0);
        }

        public void SetInactive()
        {
            SetStatusCode(2);
            SetStateCode(1);
        }

        public virtual int? GetStatusCode() { return null; }
        public virtual void SetStatusCode(int? value) {  }

        public virtual int? GetStateCode() { return null; }
        public virtual void SetStateCode(int? value) {  }


        [JsonIgnore]
        public bool IsActive { get => GetStatusCode() == 1 && GetStateCode() == 0; }
    }
}

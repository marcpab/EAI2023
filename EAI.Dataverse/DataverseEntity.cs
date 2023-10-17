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

        public virtual int? GetStatusCode() { throw new NotImplementedException(); }
        public virtual void SetStatusCode(int? value) { throw new NotImplementedException(); }

        public virtual int? GetStateCode() { throw new NotImplementedException(); }
        public virtual void SetStateCode(int? value) { throw new NotImplementedException(); }


        [JsonIgnore]
        public bool IsActive { get => GetStatusCode() == 1 && GetStateCode() == 0; }
    }
}

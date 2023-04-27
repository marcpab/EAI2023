using EAI.OData;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class PicklistAttributeMetadata
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        public string LogicalName { get; set; }
        public string MetadataId { get; set; }
        public OptionSetMetadata OptionSet { get; set; }

        public static Task<PicklistAttributeMetadata> GetPicklistAttributeMetadataAsync(ODataClient odataClient, string entity, string attribute)
            => odataClient
                    .Fetch(new ODataQuery()
                    {
                        Path = $"EntityDefinitions(LogicalName={ODataQuery.Quote(entity)})/Attributes(LogicalName={ODataQuery.Quote(attribute)})/Microsoft.Dynamics.CRM.PicklistAttributeMetadata",
                        Select = "LogicalName",
                        Expand = "OptionSet"
                    })
                    .ResultAs<PicklistAttributeMetadata>()
                    .ExecuteAsync();
    }
}

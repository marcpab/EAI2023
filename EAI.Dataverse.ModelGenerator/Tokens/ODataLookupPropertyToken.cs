using EAI.OData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class ODataLookupPropertyToken : IToken
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
        public string ReferencingRelationshipSchemaName { get; set; }

        public string ReferencingRelationshipReferencingEntityNavigationPropertyName { get; set; }

        public void Write(StringBuilder code)
        {
            code.AppendLine($"\t\t/// <summary>");
            if (!string.IsNullOrEmpty(DisplayName))
                code.AppendLine($"\t\t/// Display name: {Utils.MultilineComment(DisplayName)}");
            if (!string.IsNullOrEmpty(Description))
                code.AppendLine($"\t\t/// Description : {Utils.MultilineComment(Description)}");
            code.AppendLine($"\t\t/// Schema name : {ReferencingRelationshipSchemaName}");
            code.AppendLine($"\t\t/// </summary>");

            code.AppendLine($"\t\tpublic {nameof(ODataBind)} {ReferencingRelationshipReferencingEntityNavigationPropertyName}Lookup;");
        }
    }
}

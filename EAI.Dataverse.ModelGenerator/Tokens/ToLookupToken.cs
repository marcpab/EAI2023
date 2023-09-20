using EAI.ModelGenerator;
using EAI.OData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class ToLookupToken : IToken
    {
        public string EntityName { get; set;  }

        public void Write(StringBuilder code)
        {
            code.AppendLine($"\t\tpublic static {nameof(ODataBind)} ToLookup(Guid? guid) => guid == null ? null : new {nameof(ODataBind)}() {{EntityName = EntitySet, EntityId = guid.Value}};");
            code.AppendLine();

            code.AppendLine($"\t\tpublic {nameof(ODataBind)} ToLookup() => ToLookup({Utils.ExcapeName(EntityName)}id);");
            code.AppendLine();
        }
    }
}

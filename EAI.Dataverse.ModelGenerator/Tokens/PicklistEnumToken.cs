using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Tokens
{
    class PicklistEnumToken : IToken
    {
        public string EnumName { get; set; }

        public IEnumerable<KeyValuePair<string, int?>> Options { get; set; }
        public void Write(StringBuilder code)
        {
            code.AppendLine();
            code.AppendLine($"\t\tpublic enum {EnumName} {{");

            foreach (var option in Options)
            {
                var optionLabel = option.Key;

                if (string.IsNullOrWhiteSpace(optionLabel))
                    optionLabel = $"option_{option.Value}";

                optionLabel = Utils.Codeify(optionLabel);

                code.AppendLine($"\t\t\t{Utils.ExcapeName(optionLabel)} = {option.Value},");
            }

            code.AppendLine("\t\t}");
            code.AppendLine();
        }
    }
}

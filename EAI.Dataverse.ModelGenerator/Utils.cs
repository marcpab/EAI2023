using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EAI.Dataverse.ModelGenerator
{
    public class Utils
    {
        private static string[] _excapeNames = new[] { "is", "as", "in", "for", "new" };

        public static string Codeify(string optionLabel)
        {
            optionLabel = Regex.Replace(optionLabel, @"[^\w_]", string.Empty);

            if (Regex.IsMatch(optionLabel, @"^\d"))
                optionLabel = $"_{optionLabel}";

            return optionLabel;
        }

        public static string ExcapeName(string name)
        {
            if (_excapeNames.Contains(name))
                return $"@{name}";

            return name;
        }
    }
}

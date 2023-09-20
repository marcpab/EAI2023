using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EAI.ModelGenerator
{
    public class Utils
    {
        private static string[] _escapeNames = new[] { "is", "as", "in", "for", "new" };

        public static string Codeify(string name)
        {
            name = Regex.Replace(name, @"[^\w_]", "_");

            if (Regex.IsMatch(name, @"^\d"))
                name = $"_{name}";

            name = EscapeName(name);

            return name;
        }

        public static string EscapeName(string name)
        {
            if (_escapeNames.Contains(name))
                return $"@{name}";

            return name;
        }

        public static string MultilineComment(string value)
        {
            return value?.Replace("\n", "\n\t\t///               ");
        }

    }
}

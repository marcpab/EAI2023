using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace EAI.OData
{
    public class ODataQuery : Dictionary<string, string>
    {
        public const string filter = "$filter";
        public const string select = "$select";
        public const string top = "$top";
        public const string skip = "$skip";
        public const string expand = "$expand";
        public const string orderby = "$orderby";

        private string _path;
        private object _index;

        public string Path { get => _path; set => _path = value; }
        public object Index { get => _index; set => _index = value; }

        public string Filter { get => Get(filter); set => Set(filter, value); }
        public string Select { get => Get(select); set => Set(select, value); }
        public string Top { get => Get(top); set => Set(top, value); }
        public string Skip { get => Get(skip); set => Set(skip, value); }
        public string Expand { get => Get(expand); set => Set(expand, value); }
        public string OrderBy { get => Get(orderby); set => Set(orderby, value); }

        private void Set(string name, string value)
        {
            this[name] = value;
        }

        private string Get(string name)
        {
            string value;
            TryGetValue(name, out value);
            return value;
        }

        public static string Quote(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "null";

            return $"'{value?.Replace("'", "''")}'";
        }

        public static string Quote(Guid value)
        {
            return $"{{{value}}}";
        }

        public override string ToString()
        {
            var path = _path;

            if(path !=  null)
            {
                if (_index != null)
                    path += $"({HttpUtility.UrlEncode(_index.ToString())})";

                string query = GetQuery("&");

                if (string.IsNullOrEmpty(query))
                    return path;

                return $"{path}?{query}";

            }

            return GetQuery(";");
        }

        private string GetQuery(string separator)
        {
            return string.Join(separator, this.Select(i => $"{i.Key}={HttpUtility.UrlEncode(i.Value)}"));
        }

    }
}
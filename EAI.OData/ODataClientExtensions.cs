using EAI.Rest.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public static class ODataClientExtensions
    {
        private static Dictionary<string, string> _defaultRequestHeaders = new Dictionary<string, string>
        {
            {"Accept", "application/json" },
            {"Prefer", "return=representation"}
        };

        private static Dictionary<string, string> _deleteRequestHeaders = new Dictionary<string, string>
        {
            {"Prefer", "return=minimal"}
        };

        private const string FetchMethod = "GET";
        private const string CreateMethod = "POST";
        private const string UpdateMethod = "PATCH";
        private const string DeleteMethod = "DELETE";

        public static FluentRequest Fetch(this ODataClient client, ODataQuery query) 
        { 
            return new FluentRequest(client, query, FetchMethod, null, _defaultRequestHeaders);
        }

        public static FluentRequest Create(this ODataClient client, ODataQuery query, object content)
        {
            return new FluentRequest(client, query, CreateMethod, content, _defaultRequestHeaders);
        }

        public static FluentRequest Update(this ODataClient client, ODataQuery query, object content)
        {
            return new FluentRequest(client, query, UpdateMethod, content, _defaultRequestHeaders);
        }

        public static FluentRequest Delete(this ODataClient client, ODataQuery query)
        {
            return new FluentRequest(client, query, DeleteMethod, null, _deleteRequestHeaders);
        }
    }
}

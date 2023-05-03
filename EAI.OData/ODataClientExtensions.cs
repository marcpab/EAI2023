using EAI.Rest.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public static class ODataClientExtensions
    {
        private const string FetchMethod = "GET";
        private const string CreateMethod = "POST";
        private const string UpdateMethod = "PATCH";
        private const string DeleteMethod = "DELETE";

        public static FluentRequest Fetch(this ODataClient client, ODataQuery query) 
        { 
            return new FluentRequest(client, query, FetchMethod, null);
        }

        public static FluentRequest Create(this ODataClient client, ODataQuery query, object content)
        {
            return new FluentRequest(client, query, CreateMethod, content);
        }

        public static FluentRequest Update(this ODataClient client, ODataQuery query, object content)
        {
            return new FluentRequest(client, query, UpdateMethod, content);
        }

        public static FluentRequest Delete(this ODataClient client, ODataQuery query)
        {
            return new FluentRequest(client, query, DeleteMethod, null);
        }
    }
}

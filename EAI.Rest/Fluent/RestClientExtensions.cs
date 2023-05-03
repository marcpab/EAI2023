using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Rest.Fluent
{
    public static class RestClientExtensions
    {
        public static FluentRequest Get(this RestClient restClient, object request)
        {
            return Perform(restClient, nameof(Get), request, null);
        }

        public static FluentRequest Put(this RestClient restClient, object request, object content)
        {
            return Perform(restClient, nameof(Put), request, content);
        }

        public static FluentRequest Patch(this RestClient restClient, object request, object content)
        {
            return Perform(restClient, nameof(Patch), request, content);
        }

        public static FluentRequest Post(this RestClient restClient, object request, object content)
        {
            return Perform(restClient, nameof(Post), request, content);
        }

        public static FluentRequest Delete(this RestClient restClient, object request)
        {
            return Perform(restClient, nameof(Delete), request, null);
        }

        public static FluentRequest Perform(this RestClient restClient, string method, object request, object content)
        {
            return new FluentRequest(restClient, request, method, content);
        }


    }
}

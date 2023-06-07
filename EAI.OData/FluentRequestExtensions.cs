using EAI.Rest.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public static class FluentRequestExtensions
    {
        public static FluentResponse<IEnumerable<T>> ResultAsEnumerable<T>(this FluentRequest fluentRequest)
        {
            return new FluentResponse<IEnumerable<T>>(async () =>
            {
                var result = await fluentRequest.GetAs<ODataEnumerable<T>>();

                return result.value;
            }, fluentRequest);
        }
    }
}

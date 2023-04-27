using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EAI.General.Extensions
{
    public static class HttpContentExtensions
    {
        public static HttpContent Clone(this HttpContent content)
        {
            if (content == null)
                return null;

            var clonedContent = CloneContent(content);

            clonedContent.Headers.Clear();

            foreach (var header in content.Headers)
                clonedContent.Headers.Add(header.Key, header.Value);

            return clonedContent;
        }

        private static HttpContent CloneContent(HttpContent content)
        {
            switch (content)
            {
                case StringContent sc:
                    return new StringContent(sc.ReadAsStringAsync().Result);
                case ByteArrayContent bac:
                    return new ByteArrayContent(bac.ReadAsByteArrayAsync().Result);

                default:
                    throw new Exception($"{content.GetType()} Content type not implemented for HttpContent.Clone extension method.");
            }
        }
    }
}

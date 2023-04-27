using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task ThrowHttpException(this HttpResponseMessage response) 
        {
            if (response.IsSuccessStatusCode)
                return;

            try
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new HttpException(
                    response.RequestMessage.RequestUri, 
                    response.StatusCode, 
                    response.ReasonPhrase, 
                    response.Headers,
                    content,
                    null);
            }
            catch(Exception ex)
            {
                throw new HttpException(
                    response.RequestMessage.RequestUri,
                    response.StatusCode,
                    response.ReasonPhrase,
                    response.Headers,
                    null,
                    ex);
            }
        }
    }
}

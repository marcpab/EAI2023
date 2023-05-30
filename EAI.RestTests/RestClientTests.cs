using Xunit;
using EAI.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using EAI.OAuth;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using EAI.Rest.Fluent;

namespace EAI.Rest.Tests
{
    public class RestClientTests
    {

        [Fact()]
        public async Task SimpleSendTestAsync()
        {
            try
            {
                var oAuthRequest = new OAuthRequest()
                {
                    ClientId = "c69a7c12-591c-44cf-882f-174ede1dd20e",
                    ClientSecret = "",
                    GrantType = OAuthRequest.GrantTypeClientCredentials,
                    Scope = "https://cosmoconsultsimodulteampandi.crm4.dynamics.com/.default"
                };

                var tennantID = "9e269bb1-6557-4278-9158-94b85c699797";
                var uri = new Uri($"https://login.microsoftonline.com/{tennantID}/oauth2/v2.0/token");

                var messageHandler = new RestMessageHandler();

                messageHandler.OAuthHandler.OAuthRequest = oAuthRequest;
                messageHandler.OAuthHandler.Uri = uri;
                messageHandler.OAuthHandler.Method = HttpMethod.Post;

                var restClient = new RestClient();

                restClient.BaseUri = new Uri(@"https://cosmoconsultsimodulteampandi.crm4.dynamics.com/api/data/v9.1/");
                restClient.MessageHandler = messageHandler;

                var restRequest = new RestRequest()
                {
                    Path = "accounts",
                    Method = HttpMethod.Get,
                    Content = null
                };

                var restResult = await restClient.SendAsync(restRequest);

                var result = await restResult.GetAsStringAsync();
            }
            catch(Exception ex)
            {
                Assert.True(false, $"Test failed with error: {ex.Message}");
            }

        }

        [Fact()]
        public async Task ClientFromConfigSendTestAsync()
        {
            try
            {
                var setting = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                var configJson = GetConfigResource();

                var restClient = JsonConvert.DeserializeObject<RestClient>(configJson, setting);

                var restRequest = new RestRequest()
                {
                    Path = "accounts",
                    Method = HttpMethod.Get,
                    Content = null
                };

                var restResult = await restClient.SendAsync(restRequest);

                var result = await restResult.GetAsStringAsync();
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Test failed with error: {ex.Message}");
            }
        }

        [Fact()]
        public async Task SimpleFluentTestAsync()
        {
            try
            {

                var setting = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };


                var configJson = GetConfigResource();

                var restClient = JsonConvert.DeserializeObject<RestClient>(configJson, setting);


                var cacheTime = new TimeSpan(0, 0, 5);

                var result1 = await restClient.Get("accounts").ResultAsString().CacheFor(cacheTime).ExecuteAsync();

                var result2 = await restClient.Get("accounts").ResultAsString().CacheFor(cacheTime).ExecuteAsync();

                await Task.Delay(cacheTime + cacheTime);
                
                var result3 = await restClient.Get("accounts").ResultAsString().CacheFor(cacheTime).ExecuteAsync();

                //var created = await restClient
                //    .Patch("accounts",
                //                new account { 
                //                    number = "123", 
                //                    name = "John Doe" 
                //                })
                //    .ResultAs<account>()
                //    .ExecuteAsync();

            }
            catch (Exception ex)
            {
                Assert.True(false, $"Test failed with error: {ex.Message}");
            }
        }

        //class account
        //{
        //    public string number; public string name;
        //}

        private static string GetConfigResource()
        {
            using (var stream = typeof(RestClientTests).Assembly.GetManifestResourceStream("EAI.RestTests.config.json"))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
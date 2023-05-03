using Xunit;
using EAI.OAuth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OAuth.Tests
{
    public class OAuthClientTests
    {
        [Fact()]
        public async Task GetTokenAsyncTest()
        {
            try
            {

                var oAuthRequest = new OAuthRequest()
                {
                    ClientId = "c69a7c12-591c-44cf-882f-174ede1dd20e ",
                    ClientSecret = "PZanKz05XJ6ovUARaluG",
                    GrantType = OAuthRequest.GrantTypeClientCredentials,
                    Scope = "https://cosmoconsultsimodulteampandi.crm4.dynamics.com/.default"
                };

                var tennantID = "9e269bb1-6557-4278-9158-94b85c699797";
                var uri = new Uri($"https://login.microsoftonline.com/{tennantID}/oauth2/v2.0/token");

                var oAuthClient = new OAuthClient();

                var token = await oAuthClient.GetTokenAsync(uri, oAuthRequest.ToString());

                var bearerToken = token.access_token;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
    }
}
using EAI.OAuth;

namespace EAI.JOData
{
    public class ODataAuth
    {
        public OAuthRequest? Request { get; private set; } = null;
        public string TennantId { get; private set; } = "51f81489-12ee-4a9e-aaae-a2591f45987d"; // MS Default Id
        public Uri Uri => new($"https://login.microsoftonline.com/{TennantId}/oauth2/v2.0/token");
        public OAuthClient OAuthClient { get; private set; } = new();

        public ODataAuth(OAuthRequest request, string tennant = "Common")
        {
            Request = request;

            TennantId = tennant;
        }

        public ODataAuth(string client, string secret, string scope, string tennantId)
        {
            Request = new OAuthRequest()
            {
                ClientId = client,
                ClientSecret = secret,
                Scope = scope,
                GrantType = OAuthRequest.GrantTypeClientCredentials
            };

            TennantId = tennantId;
        }

        public ODataAuth(string client, string secret, string scope, string resource, string tennantId)
        {
            Request = new OAuthRequest()
            {
                ClientId = client,
                ClientSecret = secret,
                Scope = scope,
                Resource = resource,
                GrantType = OAuthRequest.GrantTypeClientCredentials
            };

            TennantId = tennantId;
        }

        public async Task<OAuthResponse> GetTokenAsync()
        {
            if (Request is null)
                throw new NullReferenceException("Request is null.");

            return await OAuthClient.GetTokenAsync(Uri, Request.ToString());
        }

        public static Uri GenerateScopeFromEndpoint(Uri endpoint)
        {
            if (endpoint.Segments.Last() != ".default")
                return new Uri(endpoint, ".default");

            return endpoint;
        }
    }
}

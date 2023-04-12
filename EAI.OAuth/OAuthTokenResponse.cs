using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OAuth
{
    public class OAuthTokenResponse : Dictionary<string, string>
    {
        public const string ParamTokenType = "token_type";
        public const string ParamAccessToken = "access_token";
        public const string ParamRefreshToken = "refresh_token";
        public const string ParamExpiresIn = "expires_in";
        public const string ParamExpiresOn = "expires_on";


        public string token_type { get => this[ParamTokenType]; }
        public string access_token { get => this[ParamAccessToken]; }
        public string refresh_token { get => this[ParamRefreshToken]; }
        public string expires_in { get => this[ParamExpiresIn]; }
        public string expires_on { get => this[ParamExpiresOn]; }
    }
}

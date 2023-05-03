using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.OAuth
{
    public class OAuthRequest
    {
        public const string MSDefaultClientId = "51f81489-12ee-4a9e-aaae-a2591f45987d";

        private Dictionary<string, string> _params = new Dictionary<string, string>();

        public const string ParamClientId     = "client_id";
        public const string ParamClientSecret = "client_secret";
        public const string ParamDeviceCode   = "device_code";
        public const string ParamGrantType    = "grant_type";
        public const string ParamPassword     = "password";
        public const string ParamResource     = "resource";
        public const string ParamScope        = "scope";
        public const string ParamUsername     = "username";

        public const string GrantTypePassword = "password";
        public const string GrantTypeClientCredentials = "client_credentials";
        public const string GrantTypeDeviceCode = "urn:ietf:params:oauth:grant-type:device_code";

        public string GetParam(string name)
        {
            _params.TryGetValue(name, out name);
            return name; 
        }

        public void SetValue(string name, string value) 
        { 
            _params[name]= value;
        }

        public string ClientId     { get => GetParam(ParamClientId    ); set => SetValue(ParamClientId    , value); }
        public string ClientSecret { get => GetParam(ParamClientSecret); set => SetValue(ParamClientSecret, value); }
        public string DeviceCode   { get => GetParam(ParamDeviceCode  ); set => SetValue(ParamDeviceCode  , value); }
        public string GrantType    { get => GetParam(ParamGrantType   ); set => SetValue(ParamGrantType   , value); }
        public string Password     { get => GetParam(ParamPassword    ); set => SetValue(ParamPassword    , value); }
        public string Resource     { get => GetParam(ParamResource    ); set => SetValue(ParamResource    , value); }
        public string Scope        { get => GetParam(ParamScope       ); set => SetValue(ParamScope       , value); }
        public string[] Scopes     { get => GetParam(ParamScope).Split(new char[] {' '}); set => SetValue(ParamScope, string.Join(" ", value)); }
        public string Username     { get => GetParam(ParamUsername    ); set => SetValue(ParamUsername    , value); }

        public override string ToString()
        {
            return string.Join("&", _params.Select(p => $"{p.Key}={p.Value}"));
        }
    }
}

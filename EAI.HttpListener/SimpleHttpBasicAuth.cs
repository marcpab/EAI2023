using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace EAI.HttpListener
{
    public class SimpleHttpBasicAuth : IHttpListenerAuth
    {
        public string UserNameSHA256Hex { get; set; }
        public string PasswordSHA256Hex { get; set; }


        public string? GetBasicAuthorizationHeader(NameValueCollection headers)
        {
            if (headers == null)
                return null;

            var authHeader = headers.GetValues("Authorization")?.FirstOrDefault();

            if (authHeader == null)
                return null;

            if (!authHeader.StartsWith("Basic"))
                return null;

            return authHeader;
        }

        public bool IsAuthorized(NameValueCollection headers)
        {
            var authHeader = GetBasicAuthorizationHeader(headers);
            if (string.IsNullOrEmpty(authHeader))
                return false;

            var authDataBase64 = authHeader.Remove(0, 6);

            var authData = Convert.FromBase64String(authDataBase64);

            var authString = Encoding.Default.GetString(authData);

            var authParts = authString.Split(new[] { ':' }, 2);

            var userName = authParts[0];
            userName = ComputeSHA256HexString(userName);

            if (string.Compare(userName, UserNameSHA256Hex, true) != 0)
                return false;

            var password = authParts[1];
            password = ComputeSHA256HexString(password);

            if (string.Compare(password, PasswordSHA256Hex, true) != 0)
                return false;

            return true;
        }

        private string ComputeSHA256HexString(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }


    }
}

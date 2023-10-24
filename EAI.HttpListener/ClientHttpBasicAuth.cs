using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EAI.HttpListener
{
    public class ClientHttpBasicAuth : IClientHttpAuth
    {
        private string _userName;
        private string _password;

        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        public Task<AuthenticationHeaderValue> GetAuthenticationHeaderValueAsync()
        {
            return Task.FromResult(new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_userName}:{_password}"))));
        }
    }
}

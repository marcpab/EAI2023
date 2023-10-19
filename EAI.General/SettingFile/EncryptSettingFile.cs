using EAI.General.SettingProperties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.SettingFile
{
    internal class EncryptSettingFile : ISettingFile
    {
        private bool _changed;
        private readonly JObject _jContent;

        public bool Changed { get => _changed; }

        public EncryptSettingFile(JObject jContent)
        {
            _jContent = jContent;
            _changed = false;
        }

        public Task ExecuteAsync()
        {
            var properties = JObjectSettingProperty.GetProperties("$encrypt", _jContent);
            if (properties != null && properties.Count != 0)
                using (var provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(RsaKeyValueProvider.Get());

                    foreach (var property in properties)
                    {
                        var encrypt = property.Value != null ? property.Value : property.ToString();

                        var encrypted = Encrypt(provider, encrypt);

                        property.Parent.Replace(new JObject(new JProperty("$encrypted", encrypted.ToString())));

                        _changed = true;
                    }
                }

            return Task.CompletedTask;
        }

        private string Encrypt(RSACryptoServiceProvider provider, string encrypt)
        {
            var blockSize = ((provider.KeySize - 384) / 8) + 6;
            var encryptBytes = Encoding.UTF8.GetBytes(encrypt);

            var encrypted = new StringBuilder();

            for (var i = 0; i < encryptBytes.Length; i += blockSize)
            {
                var encryptBlockBytes = new byte[Math.Min(blockSize, encryptBytes.Length - i)];

                Array.Copy(encryptBytes, i, encryptBlockBytes, 0, encryptBlockBytes.Length);

                if (encrypted.Length > 0)
                    encrypted.Append("#");

                encrypted.Append(Convert.ToBase64String(provider.Encrypt(encryptBlockBytes, true)));
            }

            return encrypted.ToString();
        }
    }
}

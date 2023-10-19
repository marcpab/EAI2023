using EAI.General.SettingProperties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.SettingFile
{
    internal class SHA256SettingFile : ISettingFile
    {
        private bool _changed;
        private readonly JObject _jContent;

        public bool Changed { get => _changed; }

        public SHA256SettingFile(JObject jContent)
        {
            _jContent = jContent;
            _changed = false;
        }

        public Task ExecuteAsync()
        {
            var properties = JObjectSettingProperty.GetProperties("$sha256", _jContent);
            if (properties != null && properties.Count != 0)
                using (var sha256 = SHA256.Create())
                    foreach (var property in properties)
                    {
                        var hexValue = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(property.Value))).Replace("-", string.Empty);

                        property.Parent.Replace(new JValue(hexValue));

                        _changed = true;
                    }

            return Task.CompletedTask;
        }

    }
}

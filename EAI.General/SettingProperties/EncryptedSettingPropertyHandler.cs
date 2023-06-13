using EAI.General.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.SettingProperties
{
    /// <summary>
    /// Handler to process $encrypted properties from the setting object
    /// </summary>
    public class EncryptedSettingPropertyHandler : ISettingsPropertyFactory
    {
        private readonly JObject _setting;

        public EncryptedSettingPropertyHandler(JObject setting)
        {
            _setting = setting;
        }


        public Task ExecuteAsync()
        {

            var properties = JObjectSettingProperty.GetProperties("$encrypted", _setting);

            if (!properties.Any())
            {
                return Task.CompletedTask;
            }

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(RsaKeyValueProvider.Get());

                foreach (var property in properties)
                    try
                    {

                        if (string.IsNullOrWhiteSpace(property.Value))
                        {
                            continue;
                        }

                        var value = property.Value;

                        var bytes = new List<byte>();
                        foreach (var part in value.Split('#'))
                        {
                            var encryptedValue = Convert.FromBase64String(part);
                            bytes.AddRange(provider.Decrypt(encryptedValue, true));
                        }

                        var decryptedValue = Encoding.UTF8.GetString(bytes.ToArray());

                        property.Parent.Replace(new JValue(decryptedValue));
                    }
                    catch (Exception ex)
                    {
                        throw new AzureException($"Error handling $encrypted property {property.Parent}", ex);
                    }
                    finally
                    {
                        property.Property.Remove();
                    }
            }


            return Task.CompletedTask;
        }

    }
}
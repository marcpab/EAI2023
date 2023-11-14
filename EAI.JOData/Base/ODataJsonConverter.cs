using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EAI.JOData.Base
{
    public class ODataJsonConverter : JsonConverter
    {
        private static readonly ParameterExpression _typeofObjectParam = Expression.Parameter(typeof(object));

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass;
        }

        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => true;
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            var token = JToken.FromObject(value);
            JObject jObject = (token as JObject)!;
            if (jObject is not null)
            {
                var valueType = value.GetType();
                string? oDataTypePropertyName = null;

                var metaObjectProvider = value as IDynamicMetaObjectProvider;
                if (metaObjectProvider is not null)
                {
                    var metaObject = metaObjectProvider.GetMetaObject(_typeofObjectParam);

                    oDataTypePropertyName =
                        metaObject
                        .GetDynamicMemberNames()
                        .Where(n =>
                                Expression.Lambda(
                                    Expression.Block(
                                        Expression.Label(CallSiteBinder.UpdateLabel),
                                        metaObject.BindGetMember(
                                            (GetMemberBinder)Binder.GetMember(0, n, valueType, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) })
                                            ).Expression
                                        ),
                                    _typeofObjectParam
                                ).Compile().DynamicInvoke(metaObjectProvider)?.GetType() == typeof(ODataType)
                                )
                        .FirstOrDefault();
                }

                if (string.IsNullOrEmpty(oDataTypePropertyName))
                    oDataTypePropertyName =
                        valueType
                        .GetFields()
                        .Where(fi => fi.GetValue(value)?.GetType() == typeof(ODataType))
                        .Select(fi => fi.Name)
                        .FirstOrDefault();

                if (string.IsNullOrEmpty(oDataTypePropertyName))
                    oDataTypePropertyName =
                        valueType
                        .GetProperties()
                        .Where(pi => pi.GetValue(value)?.GetType() == typeof(ODataType))
                        .Select(pi => pi.Name)
                        .FirstOrDefault();

                if (!string.IsNullOrEmpty(oDataTypePropertyName))
                {
                    var odataTypeJProperty = jObject[oDataTypePropertyName];
                    odataTypeJProperty?.Parent?.Replace(new JProperty("@odata.type", ((JProperty?)odataTypeJProperty?.First)?.Value));
                }

                foreach (var lookupField in valueType
                        .GetFields()
                        .Where(fi => fi.GetValue(value) is ODataLookup))
                {
                    var lookup = lookupField.GetValue(value) as ODataLookup;

                    var odataTypeJProperty = jObject[lookupField.Name];
                    odataTypeJProperty?.Parent?.Replace(new JProperty($"{lookupField.Name}@odata.bind", lookup?.ToJToken()));
                }

                foreach (var lookupProperty in valueType
                        .GetProperties()
                        .Where(pi => pi.GetValue(value) is ODataLookup))
                {
                    var lookup = lookupProperty.GetValue(value) as ODataLookup;

                    var odataTypeJProperty = jObject[lookupProperty.Name];
                    odataTypeJProperty?.Parent?.Replace(new JProperty($"{lookupProperty.Name}@odata.bind", lookup?.ToJToken()));
                }
            }

            token.WriteTo(writer);
        }
    }
}

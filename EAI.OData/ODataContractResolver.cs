using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EAI.OData
{
    class ODataContractResolver : DefaultContractResolver
    {
        private static ParameterExpression _typeofObjectParam = Expression.Parameter(typeof(object));

        public ODataContractResolver() { }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var objectContract = base.CreateObjectContract(objectType);

            var odataProperties = objectContract
                .Properties
                .Where(p => 
                            p.PropertyType == typeof(ODataType) || 
                            p.PropertyType == typeof(ODataBind))
                .ToArray();

            foreach (var prop in odataProperties)
                objectContract.Properties.Remove(prop);

            if(odataProperties.Length > 0 || typeof(IDynamicMetaObjectProvider).IsAssignableFrom(objectType))
                objectContract.ExtensionDataGetter = GetODataExtensions;

            return objectContract;
        }

        private IEnumerable<KeyValuePair<object, object>> GetODataExtensions(object value)
        {
            var dict = new Dictionary<object, object>();

            var valueType = value.GetType();
            ODataType oDataType = null;

            var metaObjectProvider = value as IDynamicMetaObjectProvider;
            if (metaObjectProvider != null)
            {
                var metaObject = metaObjectProvider.GetMetaObject(_typeofObjectParam);

                oDataType =
                    metaObject
                    .GetDynamicMemberNames()
                    .Select(name =>
                            Expression.Lambda(
                                Expression.Block(
                                    Expression.Label(CallSiteBinder.UpdateLabel),
                                    metaObject.BindGetMember(
                                        (GetMemberBinder)Binder.GetMember(0, name, valueType, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) })
                                        ).Expression
                                    ),
                                _typeofObjectParam
                            ).Compile().DynamicInvoke(metaObjectProvider) as ODataType
                            )
                    .Where(v => v != null)
                    .FirstOrDefault();

                foreach (var lookup in metaObject
                    .GetDynamicMemberNames()
                    .Select(name =>
                                new {
                                    Name = name,
                                    Lookup =
                                        Expression.Lambda(
                                            Expression.Block(
                                                Expression.Label(CallSiteBinder.UpdateLabel),
                                                metaObject.BindGetMember(
                                                    (GetMemberBinder)Binder.GetMember(0, name, valueType, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) })
                                                    ).Expression
                                                ),
                                            _typeofObjectParam
                                        ).Compile().DynamicInvoke(metaObjectProvider) as ODataBind
                                }
                            )
                        .Where(v => v.Lookup != null))
                    dict.Add(GetLookupName(lookup.Name), lookup.Lookup.ToJToken());
            }

            if (oDataType == null)
                oDataType =
                    valueType
                    .GetFields()
                    .Select(fi => fi.GetValue(value) as ODataType)
                    .Where(v => v != null)
                    .FirstOrDefault();

            if (oDataType == null)
                oDataType =
                    valueType
                    .GetProperties()
                    .Select(fi => fi.GetValue(value) as ODataType)
                    .Where(v => v != null)
                    .FirstOrDefault();

            if (oDataType != null)
                dict.Add("@odata.type", oDataType.Name);

            foreach (var lookup in valueType
                    .GetFields()
                    .Select(fi =>
                            new
                            {
                                Name = fi.Name,
                                Lookup = fi.GetValue(value) as ODataBind
                            })
                    .Where(v => v.Lookup != null))
                dict.Add(GetLookupName(lookup.Name), lookup.Lookup.ToJToken());

            foreach (var lookup in valueType
                    .GetProperties()
                    .Select(p =>
                            new
                            {
                                Name = p.Name,
                                Lookup = p.GetValue(value) as ODataBind
                            })
                    .Where(v => v.Lookup != null))
                dict.Add(GetLookupName(lookup.Name), lookup.Lookup.ToJToken());

            if (dict.Count == 0)
                return null;

            return dict;
        }

        private string GetLookupName(string name)
        {
            if (name.EndsWith(ODataBind.PropertyPostfix))
                name = name.Substring(0, name.Length - ODataBind.PropertyPostfix.Length);

            return $"{name}@odata.bind";
        }
    }
}
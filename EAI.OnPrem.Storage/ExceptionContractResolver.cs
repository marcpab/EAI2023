using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace EAI.OnPrem.Storage
{
    public class ExceptionContractResolver : DefaultContractResolver
    {
        public ExceptionContractResolver()
        {
            IgnoreSerializableInterface = true;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var c = base.CreateObjectContract(objectType);

            var isException = typeof(Exception).IsAssignableFrom(objectType);
            if (isException)
            {
                var propertyNames = c.Properties.Select(p => p.PropertyName.ToLower()).ToArray();

                var mostSpecific = objectType
                    .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(c => new
                    {
                        ConstructorInfo = c,
                        MatchingParameterNameCount = c.GetParameters().Where(p => propertyNames.Contains(p.Name.ToLower())).Count()
                    })
                    .OrderBy(e => e.MatchingParameterNameCount)
                    .Select(e => e.ConstructorInfo)
                    .LastOrDefault();

                if (mostSpecific != null)
                {
                    c.OverrideCreator = (p) => mostSpecific.Invoke(p);

                    foreach (var o in CreateConstructorParameters(mostSpecific, c.Properties))
                        c.CreatorParameters.Add(o);
                }
            }

            return c;
        }
    }
}

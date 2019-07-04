using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DBToJSON.JsonSerializers.Helpers
{
    public class FlattenToSingleValueConverter : JsonConverter
    {
        private IDictionary<Type, List<FlattenArgs>> _getValues = new Dictionary<Type, List<FlattenArgs>>();

        public void Flatten<T>(Expression<Func<T,object>> property) where T : class
        {
            var names = property.ToString().Split('.');
            if (names.Length != 2)
            {
                throw new ArgumentException("property must be a.b");
            }
            var compFunc = property.Compile();
            var fa = new FlattenArgs { PropertyName = names[1], GetValue = o => compFunc((T)o) };
            if (_getValues.TryGetValue(typeof(T), out List<FlattenArgs> fas))
            {
                fas.Add(fa);
            }
            else
            {
                _getValues.Add(typeof(T), new List<FlattenArgs> { fa });
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return _getValues.ContainsKey(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            foreach (var fa in _getValues[value.GetType()])
            {
                object res = fa.GetValue(value);

                var token = JToken.FromObject(res);
                token.WriteTo(writer);
            }
        }

        public override bool CanRead
        {
            get => false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        internal class FlattenArgs
        {
            public string PropertyName { get; set; }
            public Func<object, object> GetValue { get; set; }
        }

    }
}

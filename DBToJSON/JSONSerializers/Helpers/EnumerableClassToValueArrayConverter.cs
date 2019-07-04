using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DBToJSON.JsonSerializers.Helpers
{
    public class EnumerableClassToValueArrayConverter : JsonConverter
    {
        private IDictionary<Type, Func<object, object>> _getValues = new Dictionary<Type,Func<object,object>>();

        public void AddConverter<T>(Func<T, object> converter) {
            _getValues.Add(typeof(T),o => converter((T)o));
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(string)){ return false; }
            return objectType.GetInterfaces().Any(i=> i.IsGenericType 
                && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                && _getValues.Keys.Contains(objectType.GetGenericArguments().FirstOrDefault() ?? objectType.GetElementType()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JArray array = new JArray();
            Func<object, object> getVal = null;
            foreach (object item in (IEnumerable)value)
            {
                if (getVal == null)
                {
                    getVal = _getValues[item.GetType()];
                }
                array.Add(JToken.FromObject(getVal(item)));
            }
            array.WriteTo(writer);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

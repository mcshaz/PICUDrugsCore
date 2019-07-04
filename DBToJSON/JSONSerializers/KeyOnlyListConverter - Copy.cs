using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DBToJSON
{
    public class KeyOnlyListConverter<T,U> : KeyOnlyListConverter
    {
        public KeyOnlyListConverter() : base(typeof(T), typeof(U)) { }
    }
    public class KeyOnlyListConverter<T> : KeyOnlyListConverter
    {
        public KeyOnlyListConverter() : base(typeof(T)) { }
    }
    public class KeyOnlyListConverter: JsonConverter
    {
        private IDictionary<Type,Func<object,object>> _getValues;
        public KeyOnlyListConverter(params Type[] types)
        {
            _getValues = types.ToDictionary(t=>t,
                t=> (from p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     where p.GetCustomAttribute(typeof(KeyAttribute)) != null
                     select (Func<object,object>)p.GetValue).Single());
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

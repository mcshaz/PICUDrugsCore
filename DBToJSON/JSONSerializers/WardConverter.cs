using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using DBToJSON.SqlEntities.BolusDrugs;
using DBToJSON.SqlEntities.Infusions;

namespace DBToJSON
{
    public class WardConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType is ICollection<BolusSortOrdering> || objectType is ICollection<InfusionSortOrdering>;
        }
        private object BolusSortOrderProperty(object bsoObj)
        {
            var bso = (BolusSortOrdering)bsoObj;
            if (bso.BolusDrugId.HasValue) { return bso.BolusDrugId.Value; }
            return bso.SectionHeader;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JArray array = new JArray();
            Func<object, object> getVal = null;
            foreach (object item in (IEnumerable)value)
            {
                if (getVal == null)
                {
                    var type = item.GetType();
                    if (type == typeof(BolusSortOrdering))
                    {
                        getVal = BolusSortOrderProperty;
                    }
                    else if (type == typeof(InfusionSortOrdering))
                    {
                        getVal = o => ((InfusionSortOrdering)o).InfusionDrugId;
                    }
                    else
                    {
                        throw new NotImplementedException("not yet able to accept " + type);
                    }
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

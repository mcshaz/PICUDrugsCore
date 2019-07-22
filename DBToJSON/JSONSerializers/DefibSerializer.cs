using Microsoft.EntityFrameworkCore;
using DBToJSON.SqlEntities.BolusDrugs;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Threading.Tasks;
using DBToJSON.JsonSerializers.Helpers;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace DBToJSON.JsonSerializers
{
    public static class DefibSerializer
    {
        private static JsonSerializer _defibJsonSerializer;
        internal static JsonSerializer DefibJsonSerializer {
            get {
                if (_defibJsonSerializer == null)
                {
                    var converter = new EnumerableClassToValueArrayConverter();
                    converter.AddConverter<DefibJoule>(j => j.Joules);
                    var resolver = new DefaultContractResolver { NamingStrategy = SerializerCommonSettings.DefaultNamingStategy };
                    _defibJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    _defibJsonSerializer.Converters.Add(converter);
                }
                return _defibJsonSerializer;
            } }
        public static async Task<IEnumerable<DefibModel>> GetAndSortDefibModels(DateTime? after, DrugSqlContext db)
        {
            IQueryable<DefibModel> query = db.DefibModels.AsNoTracking().Include(m=>m.DefibJoules);
            if (after.HasValue)
            {
                query = query.Where(d => d.DateModified > after);
            }
            var data = await query.ToListAsync();
            foreach(var d in data)
            {
                d.DefibJoules = d.DefibJoules.OrderBy(j => j.Joules).ToList();
            }
            return data;
        }

    }
}

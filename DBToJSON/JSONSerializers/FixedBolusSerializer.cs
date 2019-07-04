using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using DBToJSON.JsonSerializers.Helpers;
using System.Threading.Tasks;
using DBToJSON.SqlEntities.BolusDrugs;
using System.Collections.Generic;
using DBToJSON.JSONSerializers.Helpers;

namespace DBToJSON.JsonSerializers
{
    public static class FixedBoluserializer
    {
        private static JsonSerializer _fixedBolusgJsonSerializer;
        internal static JsonSerializer FixedBolusJsonSerializer
        {
            get
            {
                if (_fixedBolusgJsonSerializer == null)
                {
                    var resolver = new SerializeExceptResolver() { NamingStrategy = SerializerCommonSettings.DefaultNamingStategy };
                    resolver.ExcludeAttribute<KeyAttribute>(mi=> mi.DeclaringType != typeof(FixedDrug));
                    resolver.ExcludeAttribute<ForeignKeyAttribute>();
                    resolver.ExcludeOtherPropByAttribute<ForeignKeyAttribute>(t=>t.Name);
                    resolver.UseIncludeHelper(Includes);
                    _fixedBolusgJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                }
                return _fixedBolusgJsonSerializer;
            }
        }
        private static IncludeHelper<FixedDrug> _includes;
        private static IncludeHelper<FixedDrug> Includes
        {
            get => _includes ?? (_includes = new IncludeHelper<FixedDrug>())
                                .Add(fd => fd.FixedDoses);
        }
        public static async Task<IEnumerable<FixedDrug>> GetFixedBoluses(DateTime? after, DrugSqlContext db)
        {
            IQueryable<FixedDrug> query = db.FixedDrugs.AddIncludes(Includes).AsNoTracking();
            if (after.HasValue)
            {
                query = query.Where(q => q.DateModified > after || q.FixedDoses.Any(b => b.DateModified > after));
            }
            return await query.ToListAsync();
        }

    }
}

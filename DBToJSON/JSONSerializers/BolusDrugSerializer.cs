using System.Linq;
using Microsoft.EntityFrameworkCore;
using DBToJSON.SqlEntities.Infusions;
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
    public static class BolusDrugSerializer
    {
        private static JsonSerializer _bolusDrugJsonSerializer;
        internal static JsonSerializer BolusDrugJsonSerializer
        {
            get
            {
                if (_bolusDrugJsonSerializer == null)
                {
                    var resolver = new SerializeExceptResolver { NamingStrategy = SerializerCommonSettings.DefaultNamingStategy };
                    resolver.ExcludeAttribute<KeyAttribute>(mi=> mi.DeclaringType != typeof(BolusDrug));
                    resolver.ExcludeAttribute<ForeignKeyAttribute>();
                    resolver.ExcludeOtherPropByAttribute<ForeignKeyAttribute>(t=>t.Name);
                    resolver.UseIncludeHelper(Includes);
                    _bolusDrugJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                }
                return _bolusDrugJsonSerializer;
            }
        }
        private static IncludeHelper<BolusDrug> _includes;
        private static IncludeHelper<BolusDrug> Includes
        {
            get => _includes ?? (_includes = new IncludeHelper<BolusDrug>())
                                .Add(id => id.BolusDoses);
        }
        public static async Task<IEnumerable<BolusDrug>> GetBolusDrugs(DateTime? after, DrugSqlContext db)
        {
            IQueryable<BolusDrug> query = db.BolusDrugs.AddIncludes(Includes).AsNoTracking();
            if (after.HasValue)
            {
                query = query.Where(q => q.DateModified > after);
            }
            return await query.ToListAsync();
        }

    }
}

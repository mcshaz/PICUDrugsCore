using System.Linq;
using Microsoft.EntityFrameworkCore;
using DBToJSON.SqlEntities.Infusions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using DBToJSON.JsonSerializers.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using DBToJSON.JSONSerializers.Helpers;

namespace DBToJSON.JsonSerializers
{
    public static class InfusionDrugSerializer
    {
        private static JsonSerializer _infusionDrugJsonSerializer;
        internal static JsonSerializer InfusionDrugJsonSerializer
        {
            get
            {
                if (_infusionDrugJsonSerializer == null)
                {
                    var converter = new FlattenToSingleValueConverter();
                    var resolver = new SerializeExceptResolver {
                        NamingStrategy = SerializerCommonSettings.DefaultNamingStategy
                    };
                    resolver.ExcludeAttribute<KeyAttribute>(mi=> mi.DeclaringType != typeof(InfusionDrug));
                    resolver.ExcludeAttribute<ForeignKeyAttribute>();
                    resolver.ExcludeOtherPropByAttribute<ForeignKeyAttribute>(t=>t.Name);
                    resolver.UseIncludeHelper(Includes);
                    _infusionDrugJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    _infusionDrugJsonSerializer.Converters.Add(converter);
                }
                return _infusionDrugJsonSerializer;
            }
        }

        private static IncludeHelper<InfusionDrug> _includes;
        private static IncludeHelper<InfusionDrug> Includes
        {
            get => _includes ?? (_includes = new IncludeHelper<InfusionDrug>()
                                .Add(id => id.DrugAmpuleConcentrations)
                                .Add(id => id.DrugReferenceSource)
                                .Add(id => id.InfusionDiluent)
                                .Add("FixedTimeDilutions.FixedTimeConcentrations")
                                .Add("VariableTimeDilutions.VariableTimeConcentrations.DoseCat"));
        }
        public static async Task<IEnumerable<InfusionDrug>> GetAndSortInfusions(DateTime? after, DrugSqlContext db)
        {
            IQueryable<InfusionDrug> query = db.InfusionDrugs.AddIncludes(Includes).AsNoTracking();
            if (after.HasValue)
            {
                query = query.Where(q => q.DateModified > after);
            }
            var data = await query.ToListAsync();
            foreach (var d in data)
            {
                foreach (var dil in d.VariableTimeDilutions)
                {
                    if (dil.VariableTimeConcentrations.Count > 1)
                    {
                        dil.VariableTimeConcentrations = dil.VariableTimeConcentrations.OrderBy(conc => (int)conc.DoseCatId).ToList();
                    }
                }
            }
            return data;
        }

    }
}

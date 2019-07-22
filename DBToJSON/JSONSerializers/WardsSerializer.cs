using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DBToJSON.SqlEntities;
using DBToJSON.SqlEntities.BolusDrugs;
using DBToJSON.SqlEntities.Infusions;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Threading.Tasks;
using DBToJSON.JsonSerializers.Helpers;
using DBToJSON.JSONSerializers.Helpers;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace DBToJSON.JsonSerializers
{
    public static class WardSerializer
    {
        private static JsonSerializer _wardJsonSerializer;
        internal static JsonSerializer WardJsonSerializer {
            get {
                if (_wardJsonSerializer == null)
                {
                    var converter = new EnumerableClassToValueArrayConverter();
                    //probably a hack to use +ve values for bolusDrug, and -ve for fixedDoseDrug
                    //all good until some poor bastard has to debug this
                    //hopefully they come across this important! comment TODO to do: make this implementation more debug proof
                    converter.AddConverter<BolusSortOrdering>(bso => bso.BolusDrugId ?? (object)bso.SectionHeader ?? -bso.FixedDrugId);
                    converter.AddConverter<InfusionSortOrdering>(iso => iso.InfusionDrugId);
                    var resolver = new DefaultContractResolver { NamingStrategy = SerializerCommonSettings.DefaultNamingStategy };
                    _wardJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    _wardJsonSerializer.Converters.Add(converter);
                }
                return _wardJsonSerializer;
            } }
        public static async Task<IEnumerable<Ward>> GetAndSortWards(DateTime? after, DrugSqlContext db)
        {
            IQueryable<Ward> query = db.Wards.AsNoTracking().Include(w => w.InfusionSortOrderings)
                .Include(w => w.BolusSortOrderings);
            if (after.HasValue)
            {
                query = query.Where(w => w.DateModified > after);
            }
            var data = await query.ToListAsync();
            foreach (var w in data)
            {
                w.InfusionSortOrderings = w.InfusionSortOrderings.OrderBy(i => i.SortOrder).ToList();
                w.BolusSortOrderings = w.BolusSortOrderings.OrderBy(b => b.SortOrder).ToList();
            }
            return data;
        }
    }
}

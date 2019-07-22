using DBToJSON.ContextHelpers;
using DBToJSON.SqlEntities;
using DBToJSON.SqlEntities.BolusDrugs;
using DBToJSON.SqlEntities.Infusions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using DBToJSON.JSONSerializers.Helpers;

namespace DBToJSON.JsonSerializers
{
    public static class GetAllJson
    {
        public static async Task WriteNosqlData(DateTime? lastServerCheckUtc, Func<TextWriter> writer)
        {
            if (lastServerCheckUtc.HasValue && lastServerCheckUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("lastServerCheckUtc must be DateTimeKind.Utc");
            }

            //in case deletion occurs between beginning 1st database query and finishing last
            var now = DateTime.UtcNow;
            Task<IEnumerable<Ward>> wardsTask;
            Task<IEnumerable<DefibModel>> defibsTask;
            Task<IEnumerable<InfusionDrug>> infusionsTask;
            Task<IEnumerable<BolusDrug>> bolusTask;
            Task<IEnumerable<DeletionsDetail>> deletionsTask;
            using (var db = new DrugSqlContext())
            {
                wardsTask = WardSerializer.GetAndSortWards(lastServerCheckUtc, db);
                defibsTask = DefibSerializer.GetAndSortDefibModels(lastServerCheckUtc, db);
                infusionsTask = InfusionDrugSerializer.GetAndSortInfusions(lastServerCheckUtc, db);
                bolusTask = BolusDrugSerializer.GetBolusDrugs(lastServerCheckUtc, db);
                deletionsTask = DeletionsSerializer.GetDeletions(lastServerCheckUtc ?? now, db);
                await Task.WhenAll(
                    wardsTask,
                    defibsTask,
                    infusionsTask,
                    bolusTask,
                    deletionsTask);
            }
            var sw = writer();
            using (var jw = new JsonTextWriter(sw))
            {
                sw.Write("{\"updateCheckStart\":" + JsonConvert.SerializeObject(now)
                       + ",\"data\":{"
                        + "\"wards\":");
                WardSerializer.WardJsonSerializer.Serialize(jw, await wardsTask);
                wardsTask = null;
                sw.Write(",\"defibModels\":");
                DefibSerializer.DefibJsonSerializer.Serialize(jw, await defibsTask);
                defibsTask = null;
                sw.Write(",\"infusionDrugs\":");
                InfusionDrugSerializer.InfusionDrugJsonSerializer.Serialize(jw, await infusionsTask);
                infusionsTask = null;
                sw.Write(",\"bolusDrugs\":");
                BolusDrugSerializer.BolusDrugJsonSerializer.Serialize(jw, await bolusTask);
                bolusTask = null;
                sw.Write(",\"deletions\":");
                DeletionsSerializer.DeletionsJsonSerializer.Serialize(jw, await deletionsTask);
                sw.Write("}}");
            }
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DBToJSON.ContextHelpers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using DBToJSON.SqlEntities.Enums;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace DBToJSON.JsonSerializers
{
    public static class DeletionsSerializer
    {
        private static JsonSerializer _deletionsJsonSerializer;
        internal static JsonSerializer DeletionsJsonSerializer
        {
            get
            {
                if (_deletionsJsonSerializer == null)
                {
                    var resolver = new DefaultContractResolver { NamingStrategy = SerializerCommonSettings.DefaultNamingStategy };
                    _deletionsJsonSerializer = new JsonSerializer
                    {
                        ContractResolver = resolver
                    };
                }
                return _deletionsJsonSerializer;
            }
        }
        public static async Task<IEnumerable<DeletionsDetail>> GetDeletions(DateTime after, DrugSqlContext db)
        {
            return await (from rd in db.RecordDeletions
                          where rd.Deleted > after
                          group rd by rd.TableId into g
                          select new DeletionsDetail {
                              Table = g.Key,
                              TableIds = g.Select(rd=>rd.IdOfDeletedRecord)
                          }).ToListAsync();
            
        }
        /// <summary>
        /// for setting up deletions table on the client
        /// </summary>
        /// <param name="utcNow"></param>
        /// <returns></returns>
        public static string SerializeEmptyDeletionsDetail(DateTime utcNow)
        {
            var emptyEnum = Enumerable.Empty<int>();
            var dd = Enum.GetValues(typeof(NosqlTable)).Cast<NosqlTable>().Select(e => new DeletionsDetail
            {
                Table = e,
                TableIds = emptyEnum
            });
            return JsonConvert.SerializeObject(dd);
        }
    }
}

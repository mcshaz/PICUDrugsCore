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
    }
}

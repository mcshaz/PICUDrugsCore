using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBToJSON
{
    public class SerializeOnlyNonKeys<T> : DefaultContractResolver
    {
        public static readonly SerializeOnlyNonKeys<T> Instance =
                                      new SerializeOnlyNonKeys<T>();

        //lest assume this called 1st for object at top of tree
        //prob wrong place, as are serializable - maybe look at source code for how notmapped handled
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var mis = base.GetSerializableMembers(objectType);
            var fks = (from p in mis
                       let a = p.GetCustomAttribute<ForeignKeyAttribute>()
                       where a != null
                       select new[] { a.Name, p.Name }).SelectMany(n=>n).ToList();
            var nonFKs = mis.Where(mi => !fks.Contains(mi.Name));
            return objectType == typeof(T)
                ? nonFKs.ToList()
                : nonFKs.Where(m => m.GetCustomAttribute(typeof(KeyAttribute)) == null).ToList();
        }
    }
}
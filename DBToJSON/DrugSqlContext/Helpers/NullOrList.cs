using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DBToJSON.Helpers
{
    internal static class NullOrList
    {
        //this is very implementation specific
        //in this case empty in -> empty out
        //any null - return null - (we should hit the database)
        public static List<T> ToListIfAllNonNull<T>(this IEnumerable<T> list) where T : class
        {
            List<T> returnVar;
            if (list is ICollection ic)
            {
                returnVar = new List<T>(ic.Count);
            }
            else
            {
                returnVar = new List<T>();
            }
            foreach (T i in list)
            {
                if (i == null)
                {
                    return null;
                }
                returnVar.Add(i);
            }
            return returnVar;
        }
        public static List<U> ToListIfAllNonNull<T, U>(this IEnumerable<T> list, Func<T, U> func) where U : class
        {
            return list.Select(func).ToListIfAllNonNull();
        }
    }
}

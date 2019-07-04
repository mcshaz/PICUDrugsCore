using System;
using System.Collections.Generic;
using System.Text;

namespace DBToJSON.Helpers
{
    static class EnumerableForEach
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> deleg)
        {
            foreach(var i in list)
            {
                deleg(i);
            }
        }
    }
}

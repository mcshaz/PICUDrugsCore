using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace XUnitTestDrugs.Utilities
{
    public enum SetResult {
        Intersect, 
        LeadingSetOnly, 
        TrailingSetOnly }
    static class LinqExtendIntercept
    {
        public static ILookup<SetResult, T> ToSets<T>(this IEnumerable<T> leadingSet, IEnumerable<T> trailingSet)
        {
            return ToSets(leadingSet,trailingSet, EqualityComparer<T>.Default);
        }
        public static ILookup<SetResult,T> ToSets<T>(this IEnumerable<T> leadingSet, IEnumerable<T> trailingSet,IEqualityComparer<T> comparer)
        {
            var leadHash = new HashSet<T>(leadingSet, comparer);
            var trailHash = new HashSet<T>(comparer);
            var intersectHash = new HashSet<T>(comparer);
            foreach (T t in trailingSet)
            {
                if (leadHash.Remove(t))
                {
                    intersectHash.Add(t);
                }
                else if (!intersectHash.Contains(t))
                {
                    trailHash.Add(t);
                }
            }
            var returnVar = new MyLookup<SetResult, T>
            {
                Create(SetResult.LeadingSetOnly, leadHash.AsReadOnly()),
                Create(SetResult.Intersect, intersectHash.AsReadOnly()),
                Create(SetResult.TrailingSetOnly, trailHash.AsReadOnly())
            };
            return returnVar;
        }
        private static Grouping<TKey, TElement> Create<TKey, TElement>(TKey key, IEnumerable<TElement> element)
        {
            return new Grouping<TKey, TElement>(key, element);
        }
    }
}

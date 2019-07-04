using System.Collections;
using System.Collections.Generic;

namespace XUnitTestDrugs.Utilities
{
    public class ReadOnlyHashSet<T> : IReadOnlyCollection<T>
    {
        HashSet<T> _set;
        public ReadOnlyHashSet(HashSet<T> set)
        {
            _set = set;
            _set.TrimExcess();
        }

        public int Count => _set.Count;

        public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    static class ReadOnlyHashSetExtensions
    {
        public static IReadOnlyCollection<T> AsReadOnly<T>(this HashSet<T> set)
        {
            return new ReadOnlyHashSet<T>(set);
        }
    }
}

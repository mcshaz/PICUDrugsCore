using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTestDrugs.Utilities
{
    class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private IEnumerable<TElement> _elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            Key = key;
            _elements = elements;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TKey Key { get; }
    }
    internal class MyLookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private IDictionary<TKey, IGrouping<TKey, TElement>> m_dict;

        internal MyLookup()
        {
            m_dict = new Dictionary<TKey, IGrouping<TKey, TElement>>();
        }

        public int Count
        {
            get => m_dict.Count;
        }

        // Returns an empty sequence if the key is not in the lookup.
        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                if (m_dict.TryGetValue(key, out IGrouping<TKey, TElement> grouping))
                {
                    return grouping;
                }

                return Enumerable.Empty<TElement>();
            }
        }

        public bool Contains(TKey key)
        {
            return m_dict.ContainsKey(key);
        }

        //
        // Adds a grouping to the lookup
        //
        // Note: The grouping should be cheap to enumerate (IGrouping extends IEnumerable), as
        // it may be enumerated multiple times depending how the user manipulates the lookup.
        // Our code must guarantee that we never attempt to insert two groupings with the same
        // key into a lookup.
        //

        internal void Add(IGrouping<TKey, TElement> grouping)
        {
            m_dict.Add(grouping.Key, grouping);
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            // First iterate over the groupings in the dictionary, and then over the default-key
            // grouping, if there is one.

            return m_dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IGrouping<TKey, TElement>>)this).GetEnumerator();
        }
    }
}

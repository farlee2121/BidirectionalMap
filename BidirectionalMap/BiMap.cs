using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BidirectionalMap
{
    public class BiMap<TForwardKey, TReverseKey>: IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>
    {
        public Indexer<TForwardKey, TReverseKey> Forward { get; private set; } = new Indexer<TForwardKey, TReverseKey>();
        public Indexer<TReverseKey, TForwardKey> Reverse { get; private set; } = new Indexer<TReverseKey, TForwardKey>();

        const string DuplicateKeyErrorMessage = "";

        public BiMap()
        {
        }
        public BiMap(IDictionary<TForwardKey, TReverseKey> oneWayMap)
        {
            Forward = new Indexer<TForwardKey, TReverseKey>(oneWayMap);
            var reversedOneWayMap = oneWayMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            Reverse = new Indexer<TReverseKey, TForwardKey>(reversedOneWayMap);
        }

        public BiMap(IEqualityComparer<TForwardKey> forwardComparer, IEqualityComparer<TReverseKey> reverseComparer)
        {
            Forward = new Indexer<TForwardKey, TReverseKey>(forwardComparer ?? EqualityComparer<TForwardKey>.Default);
            Reverse = new Indexer<TReverseKey, TForwardKey>(reverseComparer ?? EqualityComparer<TReverseKey>.Default);
        }

        public BiMap(IDictionary<TForwardKey, TReverseKey> oneWayMap, IEqualityComparer<TForwardKey> forwardComparer, IEqualityComparer<TReverseKey> reverseComparer)
        {
            Forward = new Indexer<TForwardKey, TReverseKey>(oneWayMap, forwardComparer ?? EqualityComparer<TForwardKey>.Default);
            var reversedOneWayMap = oneWayMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key, reverseComparer ?? EqualityComparer<TReverseKey>.Default);
            Reverse = new Indexer<TReverseKey, TForwardKey>(reversedOneWayMap, reverseComparer ?? EqualityComparer<TReverseKey>.Default);
        }

        public void Add(TForwardKey t1, TReverseKey t2)
        {
            if (Forward.ContainsKey(t1))
                throw new ArgumentException(DuplicateKeyErrorMessage, nameof(t1));
            if (Reverse.ContainsKey(t2))
                throw new ArgumentException(DuplicateKeyErrorMessage, nameof(t2));

            Forward.Add(t1, t2);
            Reverse.Add(t2, t1);
        }

        public bool TryAdd(TForwardKey t1, TReverseKey t2)
        {
            if (!Forward.TryAdd(t1, t2))
                return false;
            if (!Reverse.TryAdd(t2, t1))
            {
                Forward.Remove(t1); // Rollback
                return false;
            }
            return true;
        }

        public bool Remove(TForwardKey forwardKey)
        {
            if (Forward.ContainsKey(forwardKey) == false) return false;
            var reverseKey = Forward[forwardKey];
            bool success;
            if (Forward.Remove(forwardKey))
            {
                if (Reverse.Remove(reverseKey))
                {
                    success = true;
                }
                else
                {
                    Forward.Add(forwardKey, reverseKey);
                    success = false;
                }
            }
            else
            {
                success = false;
            }

            return success;
        }

        public int Count()
        {
            return Forward.Count();
        }

        IEnumerator<KeyValuePair<TForwardKey, TReverseKey>> IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>.GetEnumerator()
        {
            return Forward.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Forward.GetEnumerator();
        }

        /// <summary>
        /// Publicly read-only lookup to prevent inconsistent state between forward and reverse map lookups
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public class Indexer<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> _dictionary;

            public Indexer()
            {
                _dictionary = new Dictionary<TKey, TValue>();
            }

            public Indexer(IDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }
            public Indexer(IEqualityComparer<TKey> comparer)
            {
                _dictionary = new Dictionary<TKey, TValue>(comparer);
            }

            public Indexer(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            {
                _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer ?? EqualityComparer<TKey>.Default);
            }

            public TValue this[TKey index]
            {
                get { return _dictionary[index]; }
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            public static implicit operator Dictionary<TKey, TValue>(Indexer<TKey, TValue> indexer)
            {
                return new Dictionary<TKey, TValue>(indexer._dictionary);
            }

            internal void Add(TKey key, TValue value)
            {
                _dictionary.Add(key, value);
            }

            internal bool TryAdd(TKey key, TValue value)
            {
                if (_dictionary.ContainsKey(key)) return false;
                _dictionary.Add(key, value);
                return true;
            }

            internal bool Remove(TKey key)
            {
                return _dictionary.Remove(key);
            }

            public bool ContainsKey(TKey key)
            {
                return _dictionary.ContainsKey(key);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return _dictionary.TryGetValue(key, out value);
            }

            public IEnumerable<TKey> Keys
            {
                get
                {
                    return _dictionary.Keys;
                }
            }

            public IEnumerable<TValue> Values
            {
                get
                {
                    return _dictionary.Values;
                }
            }

            /// <summary>
            /// Deep copy lookup as a dictionary
            /// </summary>
            /// <returns></returns>
            public Dictionary<TKey, TValue> ToDictionary()
            {
                return new Dictionary<TKey, TValue>(_dictionary);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }
        }

        

        
    }
}

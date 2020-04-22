using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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

        public void Add(TForwardKey t1, TReverseKey t2)
        {
            if (Forward.ContainsKey(t1))
                throw new ArgumentException(DuplicateKeyErrorMessage, nameof(t1));
            if (Reverse.ContainsKey(t2))
                throw new ArgumentException(DuplicateKeyErrorMessage, nameof(t2));

            Forward.Add(t1, t2);
            Reverse.Add(t2, t1);
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
            return Forward.ToDictionary().GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Forward.ToDictionary().GetEnumerator();
        }

        /// <summary>
        /// Publically read-only lookup to prevent inconsistent state between forward and reverse map lookups
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="Value"></typeparam>
        public class Indexer<Key, Value>
        {
            private IDictionary<Key, Value> _dictionary;

            public Indexer()
            {
                _dictionary = new Dictionary<Key, Value>();
            }
            public Indexer(IDictionary<Key, Value> dictionary)
            {
                _dictionary = dictionary;
            }
            public Value this[Key index]
            {
                get { return _dictionary[index]; }
            }

            public static implicit operator Dictionary<Key, Value>(Indexer<Key, Value> indexer)
            {
                return new Dictionary<Key, Value>(indexer._dictionary);
            }

            internal void Add(Key key, Value value)
            {
                _dictionary.Add(key, value);
            }

            internal bool Remove(Key key)
            {
                return _dictionary.Remove(key);
            }

            internal int Count()
            {
                return _dictionary.Count;
            }

            internal bool ContainsKey(Key key)
            {
                return _dictionary.ContainsKey(key);
            }

            /// <summary>
            /// Deep copy lookup as a dictionary
            /// </summary>
            /// <returns></returns>
            public Dictionary<Key, Value> ToDictionary()
            {
                return new Dictionary<Key, Value>(_dictionary);
            }
            
        }

        

        
    }
}

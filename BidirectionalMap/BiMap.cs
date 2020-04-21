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
            Forward.Add(t1, t2);
            Reverse.Add(t2, t1);
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
                set { _dictionary[index] = value; }
            }

            public static implicit operator Dictionary<Key, Value>(Indexer<Key, Value> indexer)
            {
                return new Dictionary<Key, Value>(indexer._dictionary);
            }

            internal void Add(Key key, Value value)
            {
                _dictionary.Add(key, value);
            }
            internal int Count()
            {
                return _dictionary.Count;
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

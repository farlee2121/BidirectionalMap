using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BidirectionalMap
{
    public class BiMap<TForwardKey, TReverseKey>
    {
        public Indexer<TForwardKey, TReverseKey> Forward { get; private set; } = new Indexer<TForwardKey, TReverseKey>();
        public Indexer<TReverseKey, TForwardKey> Reverse { get; private set; } = new Indexer<TReverseKey, TForwardKey>();


        public BiMap()
        {
        }

        public void Add(TForwardKey t1, TReverseKey t2)
        {
            Forward.Add(t1, t2);
            Reverse.Add(t2, t1);
        }

        /// <summary>
        /// Publically read-only lookup to prevent inconsistent state between forward and reverse map lookups
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="Value"></typeparam>
        public class Indexer<Key, Value>
        {
            private Dictionary<Key, Value> _dictionary;

            public Indexer()
            {
                _dictionary = new Dictionary<Key, Value>();
            }
            public Indexer(Dictionary<Key, Value> dictionary)
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
                return indexer._dictionary;
            }

            internal void Add(Key key, Value value)
            {
                _dictionary.Add(key, value);
            }

            /// <summary>
            /// Deep copy lookup as a dictionary
            /// </summary>
            /// <returns></returns>
            public Dictionary<Key, Value> ToDictionary()
            {
                return _dictionary;
            }
            
        }

        

        
    }
}

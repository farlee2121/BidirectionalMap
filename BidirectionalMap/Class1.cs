using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BidirectionalMap
{
    public class BidirectionalMap<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>//, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, IDeserializationCallback, ISerializable
    {
        



        // hmm, these need to be non-editable from the outside if I want to enforce consitent state
        private Dictionary<TKey, TValue> Forward { get; set; } = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> Reverse { get; set; } = new Dictionary<TValue, TKey>();

        public ICollection<TKey> Keys => Forward.Keys;

        public ICollection<TValue> Values => Forward.Values;

        public int Count => Forward.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        //TODO: make sure set operations keep the collections the same
        public TValue this[TKey key] { get => Forward[key]; set => Forward[key] = value; }
        public TKey this[TValue key] { get => Reverse[key]; set => Reverse[key] = value; }


        public BidirectionalMap()
        {
            // question, would this be easier if I forced them to specify forward/reverse?
            // I'd probably still need to implement a custom type to prevent modifying separately. It could simplify conversion to dictionary/list though
            // how do I want to use this? Well, explicity stating forward and reverse is clearer, but I can also usually infer from what type is passed in
            // 
        }
        public BidirectionalMap(IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                Forward.Add(kvp.Key, kvp.Value);
                Reverse.Add(kvp.Value, kvp.Key);
            }
        }

        public void Add(TKey key, TValue value)
        {
            Forward.Add(key, value);
            Reverse.Add(value, key);
        }

        public bool ContainsReverseKey(TValue key)
        {
            return Reverse.ContainsKey(key);
        }
        public bool ContainsKey(TKey key)
        {
            return Forward.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            // how do I make this transactional?
            bool success;
            var value = Forward[key];
            if (Forward.Remove(key)){
                if (Reverse.Remove(Forward[key]))
                {
                    success = true;
                }
                else
                {
                    Forward.Add(key, value);
                    success = false;
                }
            }
            else
            {
                success = false;
            }

            return success;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Forward.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Forward.Add(item.Key, item.Value);
            Reverse.Add(item.Value, item.Key);
        }

        public void Clear()
        {
            Forward.Clear();
            Reverse.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }
        public bool Contains(KeyValuePair<TValue, TKey> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Forward.GetEnumerator();
        }
    }
}

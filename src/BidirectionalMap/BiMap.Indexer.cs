namespace System.Collections.Generic
{
    public partial class BiMap<TDirectKey, TReverseKey>
    {
        /// <summary>
        /// Represents a read-only dictionary. It is used to provides safely access to <see cref="BiMap{TDirectKey, TReverseKey}"/> values.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        public class Indexer<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, ICollection, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
        {
            private readonly IDictionary<TKey, TValue> _dictionary;

            public TValue this[TKey key] => _dictionary[key];
            public IEnumerable<TKey> Keys => _dictionary.Keys; // must be readonly
            public IEnumerable<TValue> Values => _dictionary.Values; // must be readonly
            public int Count => _dictionary.Count;
            bool ICollection.IsSynchronized => ((ICollection)_dictionary).IsSynchronized;
            object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

            #region Constructors

            internal Indexer()
            {
                _dictionary = new Dictionary<TKey, TValue>();
            }

            internal Indexer(IDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            }

            #endregion

            /// <summary>
            /// Creates a new instance of the <see cref="Dictionary{TKey, TValue}"/> that contains copied elements.
            /// </summary>
            /// <returns>A created dictionary.</returns>
            public Dictionary<TKey, TValue> ToDictionary() => new Dictionary<TKey, TValue>(_dictionary);

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
            
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
                _dictionary.GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)_dictionary).CopyTo(array, index);

            IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

            #region Internal methods

            internal void AddInternal(TKey key, TValue value) => _dictionary.Add(key, value);

            internal bool RemoveInternal(TKey key) => _dictionary.Remove(key);

            internal bool RemoveInternal(TKey key, out TValue value) => _dictionary.Remove(key, out value);

            internal void ClearInternal() => _dictionary.Clear();

            #endregion
        }
    }
}

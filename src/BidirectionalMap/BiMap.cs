using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a two-way dictionary with accessing elements by direct and reverse keys.
    /// </summary>
    /// <typeparam name="TDirectKey">The type of the direct keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public partial class BiMap<TDirectKey, TReverseKey>: ICollection<KeyValuePair<TDirectKey, TReverseKey>>, ICollection, IEnumerable<KeyValuePair<TDirectKey, TReverseKey>>, IEnumerable
    {
        /// <summary>
        /// Gets the read-only direct element indexer.
        /// </summary>
        public Indexer<TDirectKey, TReverseKey> Direct { get; }

        /// <summary>
        /// Gets read-only reverse element indexer.
        /// </summary>
        public Indexer<TReverseKey, TDirectKey> Reverse { get; }

        public int Count => Direct.Count;
        bool ICollection<KeyValuePair<TDirectKey, TReverseKey>>.IsReadOnly => false;
        bool ICollection.IsSynchronized => ((ICollection)Direct).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)Direct).SyncRoot;

        public BiMap()
        {
            Direct  = new Indexer<TDirectKey, TReverseKey>();
            Reverse = new Indexer<TReverseKey, TDirectKey>();
        }

        public BiMap(IDictionary<TDirectKey, TReverseKey> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            var reversedDictionary = dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            Direct  = new Indexer<TDirectKey, TReverseKey>(dictionary);
            Reverse = new Indexer<TReverseKey, TDirectKey>(reversedDictionary);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="directKey"></param>
        /// <param name="reverseKey"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(TDirectKey directKey, TReverseKey reverseKey)
        {
            if (Direct.ContainsKey(directKey))
                throw new ArgumentException("The key already exists in the collection.", nameof(directKey));

            if (Reverse.ContainsKey(reverseKey))
                throw new ArgumentException("The key already exists in the collection.", nameof(reverseKey));

            Direct.AddInternal(directKey, reverseKey);
            Reverse.AddInternal(reverseKey, directKey);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="directKey"></param>
        /// <param name="reverseKey"></param>
        /// <returns></returns>
        public bool Remove(TDirectKey directKey, out TReverseKey reverseKey)
        {
            if(Direct.RemoveInternal(directKey, out reverseKey))
                return Reverse.RemoveInternal(reverseKey); // always true
            else
                return false;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="directKey"></param>
        /// <returns></returns>
        public bool Remove(TDirectKey directKey) => Remove(directKey, out _);

        public IEnumerator<KeyValuePair<TDirectKey, TReverseKey>> GetEnumerator() =>
            Direct.GetEnumerator();

        public void Clear()
        {
            Direct.ClearInternal();
            Reverse.ClearInternal();
        }

        public bool Contains(KeyValuePair<TDirectKey, TReverseKey> item) => Direct.Contains(item);

        public bool Remove(KeyValuePair<TDirectKey, TReverseKey> item)
        {
            if (Direct.TryGetValue(item.Key, out var reversedKey) && Comparer.Default.Compare(item, reversedKey) == 0)
                return Direct.RemoveInternal(item.Key) & Reverse.RemoveInternal(reversedKey); // shoud be true
            else
                return false;
        }

        public void CopyTo(KeyValuePair<TDirectKey, TReverseKey>[] array, int arrayIndex) =>
            ((ICollection)Direct).CopyTo(array, arrayIndex);

        public void Add(KeyValuePair<TDirectKey, TReverseKey> item) => Add(item.Key, item.Value);
        
        void ICollection.CopyTo(Array array, int index) => ((ICollection)Direct).CopyTo(array, index);
        
        IEnumerator IEnumerable.GetEnumerator() => Direct.GetEnumerator();
    }
}

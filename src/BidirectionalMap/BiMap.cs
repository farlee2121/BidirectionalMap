using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a two-way dictionary with accessing elements by direct and reverse keys.
    /// </summary>
    /// <typeparam name="TDirectKey">The type of the direct keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public class BiMap<TDirectKey, TReverseKey>: IDictionary<TDirectKey, TReverseKey>, IDictionary, IReadOnlyDictionary<TDirectKey, TReverseKey>, ICollection<KeyValuePair<TDirectKey, TReverseKey>>, ICollection, IEnumerable<KeyValuePair<TDirectKey, TReverseKey>>, IEnumerable
    {
        private readonly Dictionary<TDirectKey, TReverseKey> _direct;
        private readonly Dictionary<TReverseKey, TDirectKey> _reverse;

        #region Properties

        /// <summary>
        /// Gets the read-only direct dictionary.
        /// </summary>
        public ReadOnlyDictionary<TDirectKey, TReverseKey> Direct { get; }

        /// <summary>
        /// Gets the read-only reverse dictionary.
        /// </summary>
        public ReadOnlyDictionary<TReverseKey, TDirectKey> Reverse { get; }

        public int Count => _direct.Count;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)_direct).SyncRoot;
        bool ICollection<KeyValuePair<TDirectKey, TReverseKey>>.IsReadOnly => false;
        ICollection<TDirectKey> IDictionary<TDirectKey, TReverseKey>.Keys => _direct.Keys;
        ICollection<TReverseKey> IDictionary<TDirectKey, TReverseKey>.Values => _direct.Values;
        TReverseKey IDictionary<TDirectKey, TReverseKey>.this[TDirectKey directKey]
        {
            get => Direct[directKey];
            set
            {
                if (directKey is null)
                    throw new ArgumentNullException(nameof(value));

                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                var reverseKey = value;

                if (Direct.TryGetValue(directKey, out var oldReverseKey))
                {
                    if (Reverse.ContainsKey(reverseKey))
                    {
                        if (EqualityComparer<TReverseKey>.Default.Equals(oldReverseKey, reverseKey))
                            return;
                        else
                            throw new ArgumentException("The reverse key already exists.", nameof(value));
                    }
                    else
                    {
                        _direct[directKey] = reverseKey;

                        _reverse.Remove(oldReverseKey);
                        _reverse.Add(reverseKey, directKey);
                    }
                }
                else
                {
                    Add(directKey, reverseKey);
                }
            }
        }
        IEnumerable<TDirectKey> IReadOnlyDictionary<TDirectKey, TReverseKey>.Keys => _direct.Keys;
        IEnumerable<TReverseKey> IReadOnlyDictionary<TDirectKey, TReverseKey>.Values => _direct.Values;
        TReverseKey IReadOnlyDictionary<TDirectKey, TReverseKey>.this[TDirectKey key] => _direct[key];
        ICollection IDictionary.Keys => _direct.Keys;
        ICollection IDictionary.Values => _direct.Values;
        bool IDictionary.IsReadOnly => false;
        bool IDictionary.IsFixedSize => false;
        object IDictionary.this[object directKey]
        {
            get
            {
                if (directKey is null)
                    throw new ArgumentNullException(nameof(directKey));
                
                if (!(directKey is TDirectKey))
                    throw new ArgumentException("The direct key type is incorrect.", nameof(directKey));

                return ((IDictionary<TDirectKey, TReverseKey>)this)[(TDirectKey)directKey]!;
            }
            set
            {
                if (directKey is null)
                    throw new ArgumentNullException(nameof(directKey));

                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                if (!(directKey is TDirectKey))
                    throw new ArgumentException("The direct key type is incorrect.", nameof(directKey));

                if (!(value is TReverseKey))
                    throw new ArgumentException("The reverse key type is incorrect.", nameof(value));

                ((IDictionary<TDirectKey, TReverseKey>)this)[(TDirectKey)directKey] = (TReverseKey)value;
            }
        }

        #endregion

        public BiMap()
        {
            _direct  = new Dictionary<TDirectKey, TReverseKey>();
            _reverse = new Dictionary<TReverseKey, TDirectKey>();
            Direct   = new ReadOnlyDictionary<TDirectKey, TReverseKey>(_direct);
            Reverse  = new ReadOnlyDictionary<TReverseKey, TDirectKey>(_reverse);
        }

        public BiMap(IDictionary<TDirectKey, TReverseKey> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            var reversedDictionary = dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            _direct  = new Dictionary<TDirectKey, TReverseKey>(dictionary);
            _reverse = new Dictionary<TReverseKey, TDirectKey>(reversedDictionary);
            Direct   = new ReadOnlyDictionary<TDirectKey, TReverseKey>(_direct);
            Reverse  = new ReadOnlyDictionary<TReverseKey, TDirectKey>(_reverse);
        }

        #region Methods

        public void Add(TDirectKey directKey, TReverseKey reverseKey)
        {
            if (directKey is null)
                throw new ArgumentNullException(nameof(directKey));

            if (reverseKey is null)
                throw new ArgumentNullException(nameof(reverseKey));

            if (Direct.ContainsKey(directKey))
                throw new ArgumentException("The direct key already exists.", nameof(directKey));

            if (Reverse.ContainsKey(reverseKey))
                throw new ArgumentException("The reverse key already exists.", nameof(reverseKey));

            _direct.Add(directKey, reverseKey);
            _reverse.Add(reverseKey, directKey);
        }

        public bool Remove(TDirectKey directKey, out TReverseKey reverseKey)
        {
            if (directKey is null)
                throw new ArgumentNullException(nameof(directKey));

            if (_direct.Remove(directKey, out reverseKey))
            {
                _reverse.Remove(reverseKey); // should be true
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(TDirectKey directKey) => Remove(directKey, out _);

        public void Clear()
        {
            _direct.Clear();
            _reverse.Clear();
        }

        public IEnumerator GetEnumerator() => Direct.GetEnumerator();

        IEnumerator<KeyValuePair<TDirectKey, TReverseKey>> IEnumerable<KeyValuePair<TDirectKey, TReverseKey>>.GetEnumerator() =>
            _direct.GetEnumerator();
        
        void ICollection.CopyTo(Array array, int index) => ((ICollection)_direct).CopyTo(array, index);

        void ICollection<KeyValuePair<TDirectKey, TReverseKey>>.Add(KeyValuePair<TDirectKey, TReverseKey> item) =>
            Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TDirectKey, TReverseKey>>.Remove(KeyValuePair<TDirectKey, TReverseKey> item)
        {
            if (item.Key is null)
                throw new ArgumentNullException(nameof(item), "Item key is null");
            
            if (item.Value is null)
                throw new ArgumentNullException(nameof(item), "Item value is null");

            if (((ICollection<KeyValuePair<TDirectKey, TReverseKey>>)_direct).Remove(item))
            {
                _reverse.Remove(item.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollection<KeyValuePair<TDirectKey, TReverseKey>>.Contains(KeyValuePair<TDirectKey, TReverseKey> item)
        {
            if (item.Key is null)
                throw new ArgumentNullException(nameof(item), "Item key is null");

            if (item.Value is null)
                throw new ArgumentNullException(nameof(item), "Item value is null");

            return _direct.Contains(item);
        }

        void ICollection<KeyValuePair<TDirectKey, TReverseKey>>.CopyTo(KeyValuePair<TDirectKey, TReverseKey>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TDirectKey, TReverseKey>>)_direct).CopyTo(array, arrayIndex);

        bool IReadOnlyDictionary<TDirectKey, TReverseKey>.ContainsKey(TDirectKey key) => _direct.ContainsKey(key);

        bool IReadOnlyDictionary<TDirectKey, TReverseKey>.TryGetValue(TDirectKey key, out TReverseKey value) =>
            _direct.TryGetValue(key, out value);

        bool IDictionary<TDirectKey, TReverseKey>.ContainsKey(TDirectKey key) => _direct.ContainsKey(key); 

        bool IDictionary<TDirectKey, TReverseKey>.TryGetValue(TDirectKey key, out TReverseKey value) =>
            _direct.TryGetValue(key, out value);

        void IDictionary.Add(object directKey, object reverseKey)
        {
            if (directKey is null)
                throw new ArgumentNullException(nameof(directKey));

            if (reverseKey is null)
                throw new ArgumentNullException(nameof(reverseKey));

            if (!(directKey is TDirectKey))
                throw new ArgumentException("The direct key type is incorrect.", nameof(directKey));

            if (!(reverseKey is TReverseKey))
                throw new ArgumentException("The reverse key type is incorrect.", nameof(directKey));

            Add((TDirectKey)directKey, (TReverseKey)reverseKey);
        }

        void IDictionary.Remove(object directKey)
        {
            if (directKey is null)
                throw new ArgumentNullException(nameof(directKey));

            if (!(directKey is TDirectKey))
                throw new ArgumentException("The direct key type is incorrect.", nameof(directKey));

            Remove((TDirectKey)directKey);
        }

        bool IDictionary.Contains(object directKey)
        {
            if (directKey is null)
                throw new ArgumentNullException(nameof(directKey));

            if (!(directKey is TDirectKey))
                throw new ArgumentException("The direct key type is incorrect.", nameof(directKey));

            return _direct.ContainsKey((TDirectKey)directKey);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() => _direct.GetEnumerator();

        #endregion
    }
}

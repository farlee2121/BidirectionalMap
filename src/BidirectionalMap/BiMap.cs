using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a two-way dictionary with accessing elements by forward and reverse keys.
    /// </summary>
    /// <typeparam name="TForwardKey">The type of the forward keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public class BiMap<TForwardKey, TReverseKey>: IDictionary<TForwardKey, TReverseKey>, IDictionary, IReadOnlyDictionary<TForwardKey, TReverseKey>, ICollection<KeyValuePair<TForwardKey, TReverseKey>>, ICollection, IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>, IEnumerable
    {
        private readonly Dictionary<TForwardKey, TReverseKey> _forward;
        private readonly Dictionary<TReverseKey, TForwardKey> _reverse;

        #region Properties

        /// <summary>
        /// Gets the read-only forward dictionary.
        /// </summary>
        public ReadOnlyDictionary<TForwardKey, TReverseKey> Forward { get; }

        /// <summary>
        /// Gets the read-only reverse dictionary.
        /// </summary>
        public ReadOnlyDictionary<TReverseKey, TForwardKey> Reverse { get; }

        public int Count => _forward.Count;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)_forward).SyncRoot;
        bool ICollection<KeyValuePair<TForwardKey, TReverseKey>>.IsReadOnly => false;
        ICollection<TForwardKey> IDictionary<TForwardKey, TReverseKey>.Keys => _forward.Keys;
        ICollection<TReverseKey> IDictionary<TForwardKey, TReverseKey>.Values => _forward.Values;
        TReverseKey IDictionary<TForwardKey, TReverseKey>.this[TForwardKey forwardKey]
        {
            get => Forward[forwardKey];
            set
            {
                if (forwardKey is null)
                    throw new ArgumentNullException(nameof(value));

                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                var reverseKey = value;

                if (Forward.TryGetValue(forwardKey, out var oldReverseKey))
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
                        _forward[forwardKey] = reverseKey;

                        _reverse.Remove(oldReverseKey);
                        _reverse.Add(reverseKey, forwardKey);
                    }
                }
                else
                {
                    Add(forwardKey, reverseKey);
                }
            }
        }
        IEnumerable<TForwardKey> IReadOnlyDictionary<TForwardKey, TReverseKey>.Keys => _forward.Keys;
        IEnumerable<TReverseKey> IReadOnlyDictionary<TForwardKey, TReverseKey>.Values => _forward.Values;
        TReverseKey IReadOnlyDictionary<TForwardKey, TReverseKey>.this[TForwardKey key] => _forward[key];
        ICollection IDictionary.Keys => _forward.Keys;
        ICollection IDictionary.Values => _forward.Values;
        bool IDictionary.IsReadOnly => false;
        bool IDictionary.IsFixedSize => false;
        object IDictionary.this[object forwardKey]
        {
            get
            {
                if (forwardKey is null)
                    throw new ArgumentNullException(nameof(forwardKey));
                
                if (!(forwardKey is TForwardKey))
                    throw new ArgumentException("The forward key type is incorrect.", nameof(forwardKey));

                return ((IDictionary<TForwardKey, TReverseKey>)this)[(TForwardKey)forwardKey]!;
            }
            set
            {
                if (forwardKey is null)
                    throw new ArgumentNullException(nameof(forwardKey));

                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                if (!(forwardKey is TForwardKey))
                    throw new ArgumentException("The forward key type is incorrect.", nameof(forwardKey));

                if (!(value is TReverseKey))
                    throw new ArgumentException("The reverse key type is incorrect.", nameof(value));

                ((IDictionary<TForwardKey, TReverseKey>)this)[(TForwardKey)forwardKey] = (TReverseKey)value;
            }
        }

        #endregion

        public BiMap()
        {
            _forward = new Dictionary<TForwardKey, TReverseKey>();
            _reverse = new Dictionary<TReverseKey, TForwardKey>();
            Forward  = new ReadOnlyDictionary<TForwardKey, TReverseKey>(_forward);
            Reverse  = new ReadOnlyDictionary<TReverseKey, TForwardKey>(_reverse);
        }

        public BiMap(IDictionary<TForwardKey, TReverseKey> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            var reversedDictionary = dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            _forward = new Dictionary<TForwardKey, TReverseKey>(dictionary);
            _reverse = new Dictionary<TReverseKey, TForwardKey>(reversedDictionary);
            Forward  = new ReadOnlyDictionary<TForwardKey, TReverseKey>(_forward);
            Reverse  = new ReadOnlyDictionary<TReverseKey, TForwardKey>(_reverse);
        }

        #region Methods

        public void Add(TForwardKey forwardKey, TReverseKey reverseKey)
        {
            if (forwardKey is null)
                throw new ArgumentNullException(nameof(forwardKey));

            if (reverseKey is null)
                throw new ArgumentNullException(nameof(reverseKey));

            if (Forward.ContainsKey(forwardKey))
                throw new ArgumentException("The forward key already exists.", nameof(forwardKey));

            if (Reverse.ContainsKey(reverseKey))
                throw new ArgumentException("The reverse key already exists.", nameof(reverseKey));

            _forward.Add(forwardKey, reverseKey);
            _reverse.Add(reverseKey, forwardKey);
        }

        public bool Remove(TForwardKey forwardKey, out TReverseKey reverseKey)
        {
            if (forwardKey is null)
                throw new ArgumentNullException(nameof(forwardKey));

            if (_forward.Remove(forwardKey, out reverseKey))
            {
                _reverse.Remove(reverseKey); // should be true
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(TForwardKey forwardKey) => Remove(forwardKey, out _);

        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        public IEnumerator GetEnumerator() => Forward.GetEnumerator();

        IEnumerator<KeyValuePair<TForwardKey, TReverseKey>> IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>.GetEnumerator() =>
            _forward.GetEnumerator();
        
        void ICollection.CopyTo(Array array, int index) => ((ICollection)_forward).CopyTo(array, index);

        void ICollection<KeyValuePair<TForwardKey, TReverseKey>>.Add(KeyValuePair<TForwardKey, TReverseKey> item) =>
            Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TForwardKey, TReverseKey>>.Remove(KeyValuePair<TForwardKey, TReverseKey> item)
        {
            if (item.Key is null)
                throw new ArgumentNullException(nameof(item), "Item key is null");
            
            if (item.Value is null)
                throw new ArgumentNullException(nameof(item), "Item value is null");

            if (((ICollection<KeyValuePair<TForwardKey, TReverseKey>>)_forward).Remove(item))
            {
                _reverse.Remove(item.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollection<KeyValuePair<TForwardKey, TReverseKey>>.Contains(KeyValuePair<TForwardKey, TReverseKey> item)
        {
            if (item.Key is null)
                throw new ArgumentNullException(nameof(item), "Item key is null");

            if (item.Value is null)
                throw new ArgumentNullException(nameof(item), "Item value is null");

            return _forward.Contains(item);
        }

        void ICollection<KeyValuePair<TForwardKey, TReverseKey>>.CopyTo(KeyValuePair<TForwardKey, TReverseKey>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TForwardKey, TReverseKey>>)_forward).CopyTo(array, arrayIndex);

        bool IReadOnlyDictionary<TForwardKey, TReverseKey>.ContainsKey(TForwardKey key) => _forward.ContainsKey(key);

        bool IReadOnlyDictionary<TForwardKey, TReverseKey>.TryGetValue(TForwardKey key, out TReverseKey value) =>
            _forward.TryGetValue(key, out value);

        bool IDictionary<TForwardKey, TReverseKey>.ContainsKey(TForwardKey key) => _forward.ContainsKey(key); 

        bool IDictionary<TForwardKey, TReverseKey>.TryGetValue(TForwardKey key, out TReverseKey value) =>
            _forward.TryGetValue(key, out value);

        void IDictionary.Add(object forwardKey, object reverseKey)
        {
            if (forwardKey is null)
                throw new ArgumentNullException(nameof(forwardKey));

            if (reverseKey is null)
                throw new ArgumentNullException(nameof(reverseKey));

            if (!(forwardKey is TForwardKey))
                throw new ArgumentException("The forward key type is incorrect.", nameof(forwardKey));

            if (!(reverseKey is TReverseKey))
                throw new ArgumentException("The reverse key type is incorrect.", nameof(forwardKey));

            Add((TForwardKey)forwardKey, (TReverseKey)reverseKey);
        }

        void IDictionary.Remove(object forwardKey)
        {
            if (forwardKey is null)
                throw new ArgumentNullException(nameof(forwardKey));

            if (!(forwardKey is TForwardKey))
                throw new ArgumentException("The forward key type is incorrect.", nameof(forwardKey));

            Remove((TForwardKey)forwardKey);
        }

        bool IDictionary.Contains(object forwardKey)
        {
            if (forwardKey is null)
                throw new ArgumentNullException(nameof(forwardKey));

            if (!(forwardKey is TForwardKey))
                throw new ArgumentException("The forward key type is incorrect.", nameof(forwardKey));

            return _forward.ContainsKey((TForwardKey)forwardKey);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() => _forward.GetEnumerator();

        #endregion
    }
}

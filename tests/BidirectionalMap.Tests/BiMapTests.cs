using System.Collections;

namespace BiforwardionalMap.Tests;

[TestClass]
public class BiMapTests
{
    #region Constructor tests

    [TestMethod]
    public void Constructor_NoArguments_CreatesEmptyMap()
    {
        var map = new BiMap<char, int>();

        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_NullDictionary_ThrowsArgumentNullException()
    {
        _ = new BiMap<char, int>(null!);
    }

    [TestMethod]
    public void Constructor_FilledDictionary_CreatesMapFromDictionaryValues()
    {
        var dictionary = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
        };

        var map = new BiMap<char, int>(dictionary);

        Assert.AreEqual(dictionary.Count, map.Count);
        
        Assert.AreEqual(dictionary.Count, map.Forward.Count);
        CollectionAssert.AreEqual(dictionary.Keys, map.Forward.Keys);
        CollectionAssert.AreEqual(dictionary.Values, map.Forward.Values);

        Assert.AreEqual(dictionary.Count, map.Reverse.Count);
        CollectionAssert.AreEqual(dictionary.Keys, map.Reverse.Values);
        CollectionAssert.AreEqual(dictionary.Values, map.Reverse.Keys);
    }

    [TestMethod]
    public void Constructor_FilledDictionaryWithDuplicateValues_ThrowsArgumentException()
    {
        var dictionary = new Dictionary<char, int>
        {
            { 'a', 0 },
            { 'b', 0 },
        };

        Assert.ThrowsException<ArgumentException>(() => new BiMap<char, int>(dictionary));
    }

    #endregion

    #region Method tests

    #region Add tests

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 2)]
    [DataRow('e', 3)]
    public void Add_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        map.Add(forwardKey, reverseKey);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);
        Assert.AreEqual(reverseKey, map.Forward[forwardKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(forwardKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void Add_EmptyMapAndNullKeys_ThrowsArgumentNullException(char? forwardKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>();

        Assert.ThrowsException<ArgumentNullException>(() => map.Add(forwardKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('a', 1)]
    [DataRow('a', 2)]
    [DataRow('b', 0)]
    [DataRow('b', 1)]
    [DataRow('b', 2)]
    [DataRow('c', 0)]
    [DataRow('c', 1)]
    public void Add_FilledMapAndDuplicateKeys_ThrowsArgumentException(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.ThrowsException<ArgumentException>(() => map.Add(forwardKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region ICollection<KeyValuePair<TKey, TValue>>.Add tests

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 2)]
    [DataRow('e', 3)]
    public void GenericICollectionAdd_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };
        
        var mapAsGenericICollection = ((ICollection<KeyValuePair<char, int>>)map);
        var item                    = new KeyValuePair<char, int>(forwardKey, reverseKey);

        mapAsGenericICollection.Add(item);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);
        Assert.AreEqual(reverseKey, map.Forward[forwardKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(forwardKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionAdd_EmptyMapAndNullKeys_ThrowsArgumentNullException(char? forwardKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>();
        var mapAsGenericICollection = ((ICollection<KeyValuePair<char?, int?>>)map);
        var item = new KeyValuePair<char?, int?>(forwardKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Add(item));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('a', 1)]
    [DataRow('a', 2)]
    [DataRow('b', 0)]
    [DataRow('b', 1)]
    [DataRow('b', 2)]
    [DataRow('c', 0)]
    [DataRow('c', 1)]
    public void GenericICollectionAdd_FilledMapAndDuplicateKeys_ThrowsArgumentException(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = ((ICollection<KeyValuePair<char, int>>)map);
        var item                    = new KeyValuePair<char, int>(forwardKey, reverseKey);

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericICollection.Add(item));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region IDIctionary.Add tests

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 2)]
    [DataRow('e', 3)]
    public void IDictionaryAdd_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = ((IDictionary)map);

        mapAsIDictionary.Add(forwardKey, reverseKey);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);
        Assert.AreEqual(reverseKey, map.Forward[(char)forwardKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(forwardKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void IDictionaryAdd_EmptyMapAndNullKeys_ThrowsArgumentNullException(object? forwardKey, object? reverseKey)
    {
        var map = new BiMap<char?, int?>();
        var mapAsIDictionary = ((IDictionary)map);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDictionary.Add(forwardKey!, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('a', 1)]
    [DataRow('a', 2)]
    [DataRow('b', 0)]
    [DataRow('b', 1)]
    [DataRow('b', 2)]
    [DataRow('c', 0)]
    [DataRow('c', 1)]
    public void IDictionaryAdd_FilledMapAndDuplicateKeys_ThrowsArgumentException(object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = ((IDictionary)map);

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Add(forwardKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow(0, 0)]
    [DataRow('a', 'a')]
    [DataRow("", 0f)]
    public void IDictionaryAdd_EmptyMapAndInvalidTypeKeys_ThrowsArgumentException(object forwardKey, object reverseKey)
    {
        var map              = new BiMap<char, int>();
        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Add(forwardKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    #endregion

    #region Remove tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void Remove_FilledMapAndExistingKeys_RemovesKeysSuccessfullyAndReturnsTrueAndReturnsOutExpectedReverseKey(
        char forwardKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var isSuccessfullyRemoved = map.Remove(forwardKey, out var reverseKey);

        Assert.IsTrue(isSuccessfullyRemoved);
        Assert.AreEqual(expectedReverseKey, reverseKey);
        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Forward.Count);
        Assert.IsFalse(map.Forward.ContainsKey(forwardKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey(reverseKey));
    }

    [TestMethod]
    public void Remove_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.ThrowsException<ArgumentNullException>(() => map.Remove(null));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void Remove_FilledMapAndMissingKeys_ReturnsFalse(char forwardKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var isSuccessfullyRemoved = map.Remove(forwardKey);

        Assert.IsFalse(isSuccessfullyRemoved);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region ICollection<KeyValuePair<TKey, TValue>>.Remove tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericICollectionRemove_FilledMapAndExistingKeys_RemovesKeysSuccessfullyAndReturnsTrue(
        char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };
        
        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(forwardKey, reverseKey);

        var isSuccessfullyRemoved = mapAsGenericICollection.Remove(item);

        Assert.IsTrue(isSuccessfullyRemoved);
        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Forward.Count);
        Assert.IsFalse(map.Forward.ContainsKey(forwardKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey(reverseKey));
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionRemove_FilledMapAndNullKeys_ThrowsArgumentNullException(char? forwardKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char?, int?>>)map;
        var item                    = new KeyValuePair<char?, int?>(forwardKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Remove(item));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void GenericICollectionRemove_FilledMapAndMissingKeys_ReturnsFalse(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(forwardKey, reverseKey);

        var isSuccessfullyRemoved = mapAsGenericICollection.Remove(item);

        Assert.IsFalse(isSuccessfullyRemoved);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region IDIctionary.Remove tests

    [TestMethod]
    [DataRow('a')]
    [DataRow('b')]
    public void IDictionaryRemove_FilledMapAndExistingKeys_RemovesKeysSuccessfully(object forwardKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;
        var reverseKey      = mapAsIDitionary[forwardKey]!;

        mapAsIDitionary.Remove(forwardKey);

        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Forward.Count);
        Assert.IsFalse(map.Forward.ContainsKey((char)forwardKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey((int)reverseKey));
    }

    [TestMethod]
    public void IDictionaryRemove_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDitionary.Remove(null!));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void IDictionaryRemove_FilledMapAndMissingKeys_ChangesNothing(object forwardKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;

        mapAsIDitionary.Remove(forwardKey);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(0f)]
    [DataRow("")]
    public void IDictionaryRemove_FilledMapAndInvalidTypeKeys_ThrowsArgumentException(object forwardKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDitionary.Remove(forwardKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    [TestMethod]
    public void Clear_FilledMap_ClearsElements()
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        map.Clear();

        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Forward.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    #region ICollection<KeyValuePair<TKey, TValue>>.Contains tests

    [TestMethod]
    [DataRow('a', 0, true)]
    [DataRow('b', 1, true)]
    [DataRow('a', 1, false)]
    [DataRow('b', 0, false)]
    [DataRow('c', 2, false)]
    [DataRow('d', 3, false)]
    public void GenericICollectionContains_FilledMapAndKeysAndExpectedResult_ReturnsTrue(
        char forwardKey, int reverseKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(forwardKey, reverseKey);

        var isItemExists = mapAsGenericICollection.Contains(item);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionContains_FilledMapAndNullKeys_ThrowsArgumentNullException(char? forwardKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char?, int?>>)map;
        var item                    = new KeyValuePair<char?, int?>(forwardKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Contains(item));
    }

    #endregion

    #region IDictionary.Contains tests

    [TestMethod]
    [DataRow('a', true)]
    [DataRow('b', true)]
    [DataRow('c', false)]
    [DataRow('d', false)]
    public void IDictionaryContains_FilledMapAndForwardKeysAndExpectedResult_ReturnsTrue(object forwardKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        var isItemExists = mapAsIDictionary.Contains(forwardKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void IDictionaryContains_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDictionary.Contains(null!));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(0f)]
    [DataRow("")]
    public void IDictionaryContains_FilledMapAndInvalidTypeForwardKeys_ThrowsArgumentException(object forwardKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Contains(forwardKey));
    }

    #endregion

    #region IReadOnlyDictionary<TKey, TValue>.ContainsKey tests

    [TestMethod]
    [DataRow('a', true)]
    [DataRow('b', true)]
    [DataRow('c', false)]
    [DataRow('d', false)]
    public void GenericIReadOnlyDictionaryContainsKey_FilledMapAndExistingForwardKeysAndExpectedResult_ReturnsTrue(
        char forwardKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var isItemExists = mapAsGenericIReadOnlyDictionary.ContainsKey(forwardKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void GenericIReadOnlyDictionaryContainsKey_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char?, int>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIReadOnlyDictionary.ContainsKey(null!));
    }

    #endregion

    #region IDictionary<TKey, TValue>.ContainsKey tests

    [TestMethod]
    [DataRow('a', true)]
    [DataRow('b', true)]
    [DataRow('c', false)]
    [DataRow('d', false)]
    public void GenericIDictionaryContainsKey_FilledMapAndExistingForwardKeysAndExpectedResult_ReturnsTrue(
        char forwardKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var isItemExists = mapAsGenericIDictionary.ContainsKey(forwardKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void GenericIDictionaryContainsKey_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIDictionary.ContainsKey(null!));
    }

    #endregion

    #region IReadOnlyDictionary<TKey, TValue>.TryGetValue tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIReadOnlyDictionaryTryGetValue_FilledMapAndExistingForwardKeys_ReturnsTrueAndReturnsOutExpectedReverseKey(
        char forwardKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var isKeyExists = mapAsGenericIReadOnlyDictionary.TryGetValue(forwardKey, out var reverseKey);

        Assert.IsTrue(isKeyExists);
        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIReadOnlyDictionaryTryGetValue_FilledMapAndMissingForwardKeys_ReturnsFalse(char forwardKey)
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char?, int>)map;

        var isKeyExists = mapAsGenericIReadOnlyDictionary.TryGetValue(forwardKey, out var _);

        Assert.IsFalse(isKeyExists);
    }

    #endregion

    #region IDictionary<TKey, TValue>.TryGetValue tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIDictionaryTryGetValue_FilledMapAndExistingForwardKeys_ReturnsTrueAndReturnsOutExpectedReverseKey(
        char forwardKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var isKeyExists = mapAsGenericIDictionary.TryGetValue(forwardKey, out var reverseKey);

        Assert.IsTrue(isKeyExists);
        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIDictionaryTryGetValue_FilledMapAndMissingForwardKeys_ReturnsFalse(char forwardKey)
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int>)map;

        var isKeyExists = mapAsGenericIDictionary.TryGetValue(forwardKey, out var _);

        Assert.IsFalse(isKeyExists);
    }

    #endregion

    #endregion

    #region Property test

    #region IDictionary[object] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void IDictionaryIndexerGet_FilledMapAndExistingForwardKeys_ReturnsExpectedReverseKey(
        object forwardKey, object expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        var reverseKey = mapAsGenericIDictionary[forwardKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void IDictionaryIndexerGet_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIDictionary[null!]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void IDictionaryIndexerGet_FilledMapAndMissingForwardKeys_ThrowsKeyNotFoundException(object forwardKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIDictionary[forwardKey]);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(0f)]
    [DataRow("")]
    public void IDictionaryIndexerGet_FilledMapAndInvalidTypeForwardKeys_ThrowsArgumentException(object forwardKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[forwardKey]);
    }

    #endregion

    #region IReadOnlyDictionary<TKey, TValue>[TKey] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndExistingForwardKeys_ReturnsExpectedReverseKey(
        char forwardKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var reverseKey = mapAsGenericIReadOnlyDictionary[forwardKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char?, int>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIReadOnlyDictionary[null!]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndMissingForwardKeys_ThrowsKeyNotFoundException(char forwardKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIReadOnlyDictionary[forwardKey]);
    }

    #endregion

    #region IDictionary<TKey, TValue>[TKey] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIDictionaryIndexerGet_FilledMapAndExistingForwardKeys_ReturnsExpectedReverseKey(
        char forwardKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var reverseKey = mapAsGenericIDictionary[forwardKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void GenericIDictionaryIndexerGet_FilledMapAndNullForwardKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIDictionary[null!]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIDictionaryIndexerGet_FilledMapAndMissingForwardKeys_ThrowsKeyNotFoundException(char forwardKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIDictionary[forwardKey]);
    }

    #endregion

    #region IDictionary[object] setter tests

    [TestMethod]
    [DataRow('a', 2)]
    [DataRow('b', 2)]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void IDictionaryIndexerSet_FilledMapAndExistingForwardKeysAndNonDuplicateReverseKeys_SetsReverseKeySuccessfully(
        object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        mapAsIDictionary[forwardKey] = reverseKey;

        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(reverseKey, map.Forward[(char)forwardKey]);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual(forwardKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow(null, 2)]
    [DataRow('a', null)]
    [DataRow('c', null)]
    public void IDictionaryIndexerSet_FilledMapAndNullKeys_ThrowsArgumentNullException(object? forwardKey, object? reverseKey)
    {
        var map = new BiMap<char?, int?>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDictionary[forwardKey!] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    public void IDictionaryIndexerSet_FilledMapAndDuplicateReverseKeys_ThrowsArgumentException(object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[forwardKey] = reverseKey);
    }

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void IDictionaryIndexerSet_FilledMapAndNewForwardKeysAndNonDuplicateReverseKeys_AddsKeysSuccessfully(
        object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        mapAsIDictionary[forwardKey] = reverseKey;

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);
        Assert.AreEqual(reverseKey, map.Forward[(char)forwardKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(forwardKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow('c', 0)]
    [DataRow('d', 1)]
    public void IDictionaryIndexerSet_FilledMapAndNewForwardKeysAndDuplicateReverseKeys_ThrowsArgumentException(
        object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[forwardKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow("", "")]
    [DataRow('a', 'a')]
    [DataRow('a', 0f)]
    [DataRow('a', "")]
    [DataRow(0, 0)]
    [DataRow(0f, 0)]
    [DataRow("", 0)]
    public void IDictionaryIndexerSet_FilledMapAndInvalidTypeKeys_ThrowsArgumentException(
        object forwardKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[forwardKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region IDictionary<TKey, TValue>[TKey] setter tests

    [TestMethod]
    [DataRow('a', 2)]
    [DataRow('b', 2)]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIDictionaryIndexerSet_FilledMapAndExistingForwardKeysAndNonDuplicateReverseKeys_SetsReverseKeySuccessfully(
        char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        mapAsGenericIDictionary[forwardKey] = reverseKey;

        Assert.AreEqual(2, map.Count);
        
        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(reverseKey, map.Forward[forwardKey]);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual(forwardKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow(null, 2)]
    [DataRow('a', null)]
    [DataRow('c', null)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNullKeys_ThrowsArgumentNullException(char? forwardKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int?>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIDictionary[forwardKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);
        
        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    public void GenericIDictionaryIndexerSet_FilledMapAndDuplicateReverseKeys_ThrowsArgumentException(char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[forwardKey] = reverseKey);
    }

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNewForwardKeysAndNonDuplicateReverseKeys_AddsKeysSuccessfully(
        char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        mapAsGenericIDictionary[forwardKey] = reverseKey;

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);
        Assert.AreEqual(reverseKey, map.Forward[forwardKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(forwardKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow('c', 0)]
    [DataRow('d', 1)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNewForwardKeysAndDuplicateReverseKeys_ThrowsArgumentException(
        char forwardKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[forwardKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Forward.Count);
        Assert.AreEqual(0, map.Forward['a']);
        Assert.AreEqual(1, map.Forward['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #endregion
}

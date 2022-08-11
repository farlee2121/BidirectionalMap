using System.Collections;

namespace BidirectionalMap.Tests;

[TestClass]
public class BiMapTests
{
    #region Constructor tests

    [TestMethod]
    public void Constructor_NoArguments_CreatesEmptyMap()
    {
        var map = new BiMap<char, int>();

        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Direct.Count);
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
        
        Assert.AreEqual(dictionary.Count, map.Direct.Count);
        CollectionAssert.AreEqual(dictionary.Keys, map.Direct.Keys);
        CollectionAssert.AreEqual(dictionary.Values, map.Direct.Values);

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
    public void Add_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        map.Add(directKey, reverseKey);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);
        Assert.AreEqual(reverseKey, map.Direct[directKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(directKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void Add_EmptyMapAndNullKeys_ThrowsArgumentNullException(char? directKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>();

        Assert.ThrowsException<ArgumentNullException>(() => map.Add(directKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Direct.Count);
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
    public void Add_FilledMapAndDuplicateKeys_ThrowsArgumentException(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.ThrowsException<ArgumentException>(() => map.Add(directKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
    public void GenericICollectionAdd_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };
        
        var mapAsGenericICollection = ((ICollection<KeyValuePair<char, int>>)map);
        var item                    = new KeyValuePair<char, int>(directKey, reverseKey);

        mapAsGenericICollection.Add(item);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);
        Assert.AreEqual(reverseKey, map.Direct[directKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(directKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionAdd_EmptyMapAndNullKeys_ThrowsArgumentNullException(char? directKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>();
        var mapAsGenericICollection = ((ICollection<KeyValuePair<char?, int?>>)map);
        var item = new KeyValuePair<char?, int?>(directKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Add(item));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Direct.Count);
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
    public void GenericICollectionAdd_FilledMapAndDuplicateKeys_ThrowsArgumentException(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = ((ICollection<KeyValuePair<char, int>>)map);
        var item                    = new KeyValuePair<char, int>(directKey, reverseKey);

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericICollection.Add(item));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
    public void IDictionaryAdd_FilledMapAndNonDuplicateKeys_AddsKeysSuccessfully(object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = ((IDictionary)map);

        mapAsIDictionary.Add(directKey, reverseKey);

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);
        Assert.AreEqual(reverseKey, map.Direct[(char)directKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(directKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void IDictionaryAdd_EmptyMapAndNullKeys_ThrowsArgumentNullException(object? directKey, object? reverseKey)
    {
        var map = new BiMap<char?, int?>();
        var mapAsIDictionary = ((IDictionary)map);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDictionary.Add(directKey!, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Direct.Count);
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
    public void IDictionaryAdd_FilledMapAndDuplicateKeys_ThrowsArgumentException(object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = ((IDictionary)map);

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Add(directKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow(0, 0)]
    [DataRow('a', 'a')]
    [DataRow("", 0f)]
    public void IDictionaryAdd_EmptyMapAndInvalidTypeKeys_ThrowsArgumentException(object directKey, object reverseKey)
    {
        var map              = new BiMap<char, int>();
        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Add(directKey, reverseKey));

        // checking that nothing has changed
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(0, map.Direct.Count);
        Assert.AreEqual(0, map.Reverse.Count);
    }

    #endregion

    #region Remove tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void Remove_FilledMapAndExistingKeys_RemovesKeysSuccessfullyAndReturnsTrueAndReturnsOutExpectedReverseKey(
        char directKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var isSuccessfullyRemoved = map.Remove(directKey, out var reverseKey);

        Assert.IsTrue(isSuccessfullyRemoved);
        Assert.AreEqual(expectedReverseKey, reverseKey);
        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Direct.Count);
        Assert.IsFalse(map.Direct.ContainsKey(directKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey(reverseKey));
    }

    [TestMethod]
    public void Remove_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.ThrowsException<ArgumentNullException>(() => map.Remove(null));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void Remove_FilledMapAndMissingKeys_ReturnsFalse(char directKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var isSuccessfullyRemoved = map.Remove(directKey);

        Assert.IsFalse(isSuccessfullyRemoved);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
        char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };
        
        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(directKey, reverseKey);

        var isSuccessfullyRemoved = mapAsGenericICollection.Remove(item);

        Assert.IsTrue(isSuccessfullyRemoved);
        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Direct.Count);
        Assert.IsFalse(map.Direct.ContainsKey(directKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey(reverseKey));
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionRemove_FilledMapAndNullKeys_ThrowsArgumentNullException(char? directKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char?, int?>>)map;
        var item                    = new KeyValuePair<char?, int?>(directKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Remove(item));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void GenericICollectionRemove_FilledMapAndMissingKeys_ReturnsFalse(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(directKey, reverseKey);

        var isSuccessfullyRemoved = mapAsGenericICollection.Remove(item);

        Assert.IsFalse(isSuccessfullyRemoved);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #region IDIctionary.Remove tests

    [TestMethod]
    [DataRow('a')]
    [DataRow('b')]
    public void IDictionaryRemove_FilledMapAndExistingKeys_RemovesKeysSuccessfully(object directKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;
        var reverseKey      = mapAsIDitionary[directKey]!;

        mapAsIDitionary.Remove(directKey);

        Assert.AreEqual(1, map.Count);

        Assert.AreEqual(1, map.Direct.Count);
        Assert.IsFalse(map.Direct.ContainsKey((char)directKey));

        Assert.AreEqual(1, map.Reverse.Count);
        Assert.IsFalse(map.Reverse.ContainsKey((int)reverseKey));
    }

    [TestMethod]
    public void IDictionaryRemove_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void IDictionaryRemove_FilledMapAndMissingKeys_ChangesNothing(object directKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;

        mapAsIDitionary.Remove(directKey);

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(0f)]
    [DataRow("")]
    public void IDictionaryRemove_FilledMapAndInvalidTypeKeys_ThrowsArgumentException(object directKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDitionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDitionary.Remove(directKey));

        // checking that nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
        Assert.AreEqual(0, map.Direct.Count);
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
        char directKey, int reverseKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char, int>>)map;
        var item                    = new KeyValuePair<char, int>(directKey, reverseKey);

        var isItemExists = mapAsGenericICollection.Contains(item);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow('a', null)]
    public void GenericICollectionContains_FilledMapAndNullKeys_ThrowsArgumentNullException(char? directKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericICollection = (ICollection<KeyValuePair<char?, int?>>)map;
        var item                    = new KeyValuePair<char?, int?>(directKey, reverseKey);

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericICollection.Contains(item));
    }

    #endregion

    #region IDictionary.Contains tests

    [TestMethod]
    [DataRow('a', true)]
    [DataRow('b', true)]
    [DataRow('c', false)]
    [DataRow('d', false)]
    public void IDictionaryContains_FilledMapAndDirectKeysAndExpectedResult_ReturnsTrue(object directKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        var isItemExists = mapAsIDictionary.Contains(directKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void IDictionaryContains_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void IDictionaryContains_FilledMapAndInvalidTypeDirectKeys_ThrowsArgumentException(object directKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary.Contains(directKey));
    }

    #endregion

    #region IReadOnlyDictionary<TKey, TValue>.ContainsKey tests

    [TestMethod]
    [DataRow('a', true)]
    [DataRow('b', true)]
    [DataRow('c', false)]
    [DataRow('d', false)]
    public void GenericIReadOnlyDictionaryContainsKey_FilledMapAndExistingDirectKeysAndExpectedResult_ReturnsTrue(
        char directKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var isItemExists = mapAsGenericIReadOnlyDictionary.ContainsKey(directKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void GenericIReadOnlyDictionaryContainsKey_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void GenericIDictionaryContainsKey_FilledMapAndExistingDirectKeysAndExpectedResult_ReturnsTrue(
        char directKey, bool expectedResult)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var isItemExists = mapAsGenericIDictionary.ContainsKey(directKey);

        Assert.AreEqual(expectedResult, isItemExists);
    }

    [TestMethod]
    public void GenericIDictionaryContainsKey_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void GenericIReadOnlyDictionaryTryGetValue_FilledMapAndExistingDirectKeys_ReturnsTrueAndReturnsOutExpectedReverseKey(
        char directKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var isKeyExists = mapAsGenericIReadOnlyDictionary.TryGetValue(directKey, out var reverseKey);

        Assert.IsTrue(isKeyExists);
        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIReadOnlyDictionaryTryGetValue_FilledMapAndMissingDirectKeys_ReturnsFalse(char directKey)
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char?, int>)map;

        var isKeyExists = mapAsGenericIReadOnlyDictionary.TryGetValue(directKey, out var _);

        Assert.IsFalse(isKeyExists);
    }

    #endregion

    #region IDictionary<TKey, TValue>.TryGetValue tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIDictionaryTryGetValue_FilledMapAndExistingDirectKeys_ReturnsTrueAndReturnsOutExpectedReverseKey(
        char directKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var isKeyExists = mapAsGenericIDictionary.TryGetValue(directKey, out var reverseKey);

        Assert.IsTrue(isKeyExists);
        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    [DataRow('c')]
    [DataRow('d')]
    public void GenericIDictionaryTryGetValue_FilledMapAndMissingDirectKeys_ReturnsFalse(char directKey)
    {
        var map = new BiMap<char?, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int>)map;

        var isKeyExists = mapAsGenericIDictionary.TryGetValue(directKey, out var _);

        Assert.IsFalse(isKeyExists);
    }

    #endregion

    #endregion

    #region Property test

    #region IDictionary[object] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void IDictionaryIndexerGet_FilledMapAndExistingDirectKeys_ReturnsExpectedReverseKey(
        object directKey, object expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        var reverseKey = mapAsGenericIDictionary[directKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void IDictionaryIndexerGet_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void IDictionaryIndexerGet_FilledMapAndMissingDirectKeys_ThrowsKeyNotFoundException(object directKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIDictionary[directKey]);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(0f)]
    [DataRow("")]
    public void IDictionaryIndexerGet_FilledMapAndInvalidTypeDirectKeys_ThrowsArgumentException(object directKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[directKey]);
    }

    #endregion

    #region IReadOnlyDictionary<TKey, TValue>[TKey] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndExistingDirectKeys_ReturnsExpectedReverseKey(
        char directKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        var reverseKey = mapAsGenericIReadOnlyDictionary[directKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void GenericIReadOnlyDictionaryIndexerGet_FilledMapAndMissingDirectKeys_ThrowsKeyNotFoundException(char directKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIReadOnlyDictionary = (IReadOnlyDictionary<char, int>)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIReadOnlyDictionary[directKey]);
    }

    #endregion

    #region IDictionary<TKey, TValue>[TKey] getter tests

    [TestMethod]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void GenericIDictionaryIndexerGet_FilledMapAndExistingDirectKeys_ReturnsExpectedReverseKey(
        char directKey, int expectedReverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        var reverseKey = mapAsGenericIDictionary[directKey];

        Assert.AreEqual(expectedReverseKey, reverseKey);
    }

    [TestMethod]
    public void GenericIDictionaryIndexerGet_FilledMapAndNullDirectKey_ThrowsArgumentNullException()
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
    public void GenericIDictionaryIndexerGet_FilledMapAndMissingDirectKeys_ThrowsKeyNotFoundException(char directKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<KeyNotFoundException>(() => mapAsGenericIDictionary[directKey]);
    }

    #endregion

    #region IDictionary[object] setter tests

    [TestMethod]
    [DataRow('a', 2)]
    [DataRow('b', 2)]
    [DataRow('a', 0)]
    [DataRow('b', 1)]
    public void IDictionaryIndexerSet_FilledMapAndExistingDirectKeysAndNonDuplicateReverseKeys_SetsReverseKeySuccessfully(
        object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        mapAsIDictionary[directKey] = reverseKey;

        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(reverseKey, map.Direct[(char)directKey]);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual(directKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow(null, 2)]
    [DataRow('a', null)]
    [DataRow('c', null)]
    public void IDictionaryIndexerSet_FilledMapAndNullKeys_ThrowsArgumentNullException(object? directKey, object? reverseKey)
    {
        var map = new BiMap<char?, int?>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsIDictionary[directKey!] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    public void IDictionaryIndexerSet_FilledMapAndDuplicateReverseKeys_ThrowsArgumentException(object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[directKey] = reverseKey);
    }

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void IDictionaryIndexerSet_FilledMapAndNewDirectKeysAndNonDuplicateReverseKeys_AddsKeysSuccessfully(
        object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        mapAsIDictionary[directKey] = reverseKey;

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);
        Assert.AreEqual(reverseKey, map.Direct[(char)directKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(directKey, map.Reverse[(int)reverseKey]);
    }

    [TestMethod]
    [DataRow('c', 0)]
    [DataRow('d', 1)]
    public void IDictionaryIndexerSet_FilledMapAndNewDirectKeysAndDuplicateReverseKeys_ThrowsArgumentException(
        object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[directKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
        object directKey, object reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsIDictionary = (IDictionary)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsIDictionary[directKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

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
    public void GenericIDictionaryIndexerSet_FilledMapAndExistingDirectKeysAndNonDuplicateReverseKeys_SetsReverseKeySuccessfully(
        char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        mapAsGenericIDictionary[directKey] = reverseKey;

        Assert.AreEqual(2, map.Count);
        
        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(reverseKey, map.Direct[directKey]);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual(directKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(null, 0)]
    [DataRow(null, 2)]
    [DataRow('a', null)]
    [DataRow('c', null)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNullKeys_ThrowsArgumentNullException(char? directKey, int? reverseKey)
    {
        var map = new BiMap<char?, int?>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char?, int?>)map;

        Assert.ThrowsException<ArgumentNullException>(() => mapAsGenericIDictionary[directKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);
        
        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    [TestMethod]
    [DataRow('a', 1)]
    [DataRow('b', 0)]
    public void GenericIDictionaryIndexerSet_FilledMapAndDuplicateReverseKeys_ThrowsArgumentException(char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[directKey] = reverseKey);
    }

    [TestMethod]
    [DataRow('c', 2)]
    [DataRow('d', 3)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNewDirectKeysAndNonDuplicateReverseKeys_AddsKeysSuccessfully(
        char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        mapAsGenericIDictionary[directKey] = reverseKey;

        Assert.AreEqual(3, map.Count);

        Assert.AreEqual(3, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);
        Assert.AreEqual(reverseKey, map.Direct[directKey]);

        Assert.AreEqual(3, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
        Assert.AreEqual(directKey, map.Reverse[reverseKey]);
    }

    [TestMethod]
    [DataRow('c', 0)]
    [DataRow('d', 1)]
    public void GenericIDictionaryIndexerSet_FilledMapAndNewDirectKeysAndDuplicateReverseKeys_ThrowsArgumentException(
        char directKey, int reverseKey)
    {
        var map = new BiMap<char, int>
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var mapAsGenericIDictionary = (IDictionary<char, int>)map;

        Assert.ThrowsException<ArgumentException>(() => mapAsGenericIDictionary[directKey] = reverseKey);

        // checking nothing has changed
        Assert.AreEqual(2, map.Count);

        Assert.AreEqual(2, map.Direct.Count);
        Assert.AreEqual(0, map.Direct['a']);
        Assert.AreEqual(1, map.Direct['b']);

        Assert.AreEqual(2, map.Reverse.Count);
        Assert.AreEqual('a', map.Reverse[0]);
        Assert.AreEqual('b', map.Reverse[1]);
    }

    #endregion

    #endregion
}

using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BidirectionalMap.Tests
{
    public class BiMapTests
    {
        // Tests
        // - make sure I can add and remove while staying consistent (via kvp, add separate k and v, mutations of indexes)
        // - make sure contains always
        // - make sure it's 1-to-1 / reversible 
        // - make sure that the indexers don't mutate when returned as a dictionary (deep copy semantic)

        [Fact]
        public void EmptyConstructorInitializesToEmptyMap()
        {
            var map = new BiMap<int, int>();

            Assert.Empty(map);
            Assert.Empty(map.Direct);
            Assert.Empty(map.Reverse);
        }

        [Fact]
        public void AddKeyValueMap()
        {
            var map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.Single(map);
            Assert.Equal(2, map.Direct[1]);
            Assert.Equal(1, map.Reverse[2]);
        }

        [Fact]
        public void ReAddExistingKey()
        {
            var map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.Throws<ArgumentException>(() => map.Add(1, 2));
            Assert.Throws<ArgumentException>(() => map.Add(1, 5));
            Assert.Throws<ArgumentException>(() => map.Add(5, 2));
        }

        [Fact]
        public void RemoveMapItem()
        {
            var map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.True(map.Remove(1));
            
            Assert.False(map.Direct.ContainsKey(1));
            Assert.Throws<KeyNotFoundException>(() => map.Direct[1]);

            Assert.False(map.Reverse.ContainsKey(2));
            Assert.Throws<KeyNotFoundException>(() => map.Reverse[2]);
        }

        [Fact]
        public void RemoveInvalidItem()
        {
            var map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.False(map.Remove(2));

            Assert.Equal(2, map.Direct[1]);
            Assert.Equal(1, map.Reverse[2]);
        }

        [Fact]
        public void RemoveFromEmptyMap()
        {
            var map = new BiMap<int, int>();

            Assert.False(map.Remove(1));
        }

        [Fact]
        public void Contains_ValidKey()
        {
            var map = new BiMap<int, int>();

            map.Add(1, 2);
            Assert.True(map.Direct.ContainsKey(1));
            Assert.True(map.Reverse.ContainsKey(2));
        }

        [Fact]
        public void Contains_InvalidKey()
        {
            var map = new BiMap<int, int>();

            Assert.False(map.Direct.ContainsKey(1));
            Assert.False(map.Reverse.ContainsKey(2));
        }

        [Fact]
        public void IndexerToDictionaryDoesNotMutateOriginal()
        {
            var map = new BiMap<int, int>();

            var dictionary = map.Direct.ToDictionary();
            dictionary.Add(1, 1);

            Assert.Empty(map.Direct);

            var reversedDictionary = map.Reverse.ToDictionary();
            reversedDictionary.Add(1, 1);

            Assert.Empty(map.Reverse);
        }

        [Fact]
        public void GetIndexerKeysAndValues()
        {
            var map = new BiMap<int, string>
            {
                { 1, "1" },
                { 2, "2" },
            };

            var forwardKeys = new[] { 1, 2 };
            Assert.Equal(forwardKeys, map.Direct.Keys);
            Assert.Equal(forwardKeys, map.Reverse.Values);

            var reverseKeys = new[] { "1", "2" };
            Assert.Equal(reverseKeys, map.Reverse.Keys);
            Assert.Equal(reverseKeys, map.Direct.Values);
        }

        [Fact]
        public void Constructor_InstantiateValid()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "1" },
                { 2, "2" },
            };

            var map = new BiMap<int, string>(dictionary);

            Assert.Equal(dictionary.Count, map.Count);
            Assert.Equal("1", map.Direct[1]);
            Assert.Equal("2", map.Direct[2]);
            Assert.Equal(1, map.Reverse["1"]);
            Assert.Equal(2, map.Reverse["2"]);
        }

        [Fact]
        public void Constructor_DuplicatKey()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "1" },
                { 2, "1" },
            };

            Assert.Throws<ArgumentException>(() => new BiMap<int, string>(dictionary));
        }

        [Fact]
        public void Initializer_InstantiateValid()
        {
            //QUESTION: Should I instead run the full test suite for every initialization method?
            BiMap<int, string> map = new BiMap<int, string>()
            {
                {1, "1" },
                {2, "2" },
                {3, "3" },
            };

            Assert.Equal(3, map.Count());
            Assert.Equal("1", map.Direct[1]);
            Assert.Equal("2", map.Direct[2]);
            Assert.Equal("3", map.Direct[3]);
            Assert.Equal(1, map.Reverse["1"]);
            Assert.Equal(2, map.Reverse["2"]);
            Assert.Equal(3, map.Reverse["3"]);
        }
    }
}

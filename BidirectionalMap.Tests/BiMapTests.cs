using System;
using Xunit;
using BidirectionalMap;
using System.Collections.Generic;

namespace BidirectionalMap.Tests
{
    public class BiMapTests
    {
        // Tests
        // - make sure I can add and remove while staying consistent (via kvp, add separate k and v, mutations of indexes)
        // - make sure contains always
        // - mmake sure it's 1-to-1 / reversible 
        // - make sure that the indexers don't mutate when returned as a dictionary (deep copy semantic)

        [Fact]
        public void EmptyConstructorInitializesToEmptyMap()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            Assert.Empty(map);
            Assert.Empty(map.Forward.ToDictionary());
            Assert.Empty(map.Reverse.ToDictionary());
        }

        [Fact]
        public void AddKeyValueMap()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            map.Add(1, 2);
            Assert.Equal(2, map.Forward[1]);
            Assert.Equal(1, map.Reverse[2]);
            Assert.Single(map);
        }

        [Fact]
        public void ReAddExistingKey()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.Throws<ArgumentException>(() => {
                map.Add(1, 1);
            });
        }

        [Fact]
        public void ReAddExistingReverseKey()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            map.Add(1, 2);
            Assert.Throws<ArgumentException>(() =>{
                map.Add(2, 2);
            });
        }

        [Fact]
        public void RemoveMapItem()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.Equal(2, map.Forward[1]);
            Assert.Equal(1, map.Reverse[2]);

            map.Remove(1);

            //Make sure I can't fetch after removing
            Assert.False(map.Forward.ContainsKey(1));
            Assert.Throws<KeyNotFoundException>(() => {
                var val = map.Forward[1];
            });

            Assert.False(map.Reverse.ContainsKey(2));
            Assert.Throws<KeyNotFoundException>(() => {
                var reverseVal = map.Reverse[2];
            });
        }

        [Fact]
        public void RemoveInvalidItem()
        {
            BiMap<int, int> map = new BiMap<int, int>();
            map.Add(1, 2);

            Assert.Equal(2, map.Forward[1]);
            Assert.Equal(1, map.Reverse[2]);

            Assert.False(map.Remove(2));

            Assert.Equal(2, map.Forward[1]);
            Assert.Equal(1, map.Reverse[2]);
        }

        [Fact] 
        public void RemoveFromEmptyMap()
        {
            BiMap<int, int> map = new BiMap<int, int>();

            Assert.False(map.Remove(1));
        }

        [Fact]
        public void Contains_ValidKey()
        {
            BiMap<int, int> map = new BiMap<int, int>();

            map.Add(1, 2);
            Assert.True(map.Forward.ContainsKey(1));
            Assert.True(map.Reverse.ContainsKey(2));
        }

        [Fact]
        public void Contains_InvalidKey()
        {
            BiMap<int, int> map = new BiMap<int, int>();

            Assert.False(map.Forward.ContainsKey(1));
            Assert.False(map.Reverse.ContainsKey(2));
        }

        [Fact]
        public void OneToOneReversible()
        {

            // this would be a good use of fscheck
            BiMap<int, string> map = new BiMap<int, string>();
            throw new NotImplementedException();
        }

        [Fact]
        public void ForwardAndReverseOperationallyConsistent()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void InstantiateFromConstructor()
        {
            var dict = new Dictionary<int, string>
            {
                {1, "1" },
                {2, "2" },
                {3, "3" },
            };
            BiMap<int, string> map = new BiMap<int, string>(dict);

            Assert.Equal(3, map.Count());
            Assert.Equal("1", map.Forward[1]);
            Assert.Equal("2", map.Forward[2]);
            Assert.Equal("3", map.Forward[3]);
            Assert.Equal(1, map.Reverse["1"]);
            Assert.Equal(2, map.Reverse["2"]);
            Assert.Equal(3, map.Reverse["3"]);
        }

        [Fact]
        public void InstantiateFromInitializer()
        {
            //QUESTION: Should I instead run the full test suite for every initialization method?
            BiMap<int, string> map = new BiMap<int, string>()
            {
                {1, "1" },
                {2, "2" },
                {3, "3" },
            };

            Assert.Equal(3, map.Count());
            Assert.Equal("1", map.Forward[1]);
            Assert.Equal("2", map.Forward[2]);
            Assert.Equal("3", map.Forward[3]);
            Assert.Equal(1, map.Reverse["1"]);
            Assert.Equal(2, map.Reverse["2"]);
            Assert.Equal(3, map.Reverse["3"]);
        }
    }
}
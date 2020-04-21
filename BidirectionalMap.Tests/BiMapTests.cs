using System;
using Xunit;
using BidirectionalMap;
using System.Collections.Generic;

namespace BidirectionalMap.Tests
{
    public class BiMapTests
    {
        // Tests
        // - make sure i can instantiate like a dictionary (given a set of keyvaluepairs)
        // - make sure I can instantiate throught the constructor with various types
        // - make sure I can add and remove while staying consistent (via kvp, add separate k and v, mutations of indexes)
        // - make sure contains always
        // - mmake sure it's 1-to-1 / reversible 
        // - test mapping to same value type
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

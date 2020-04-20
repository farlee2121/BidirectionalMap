using System;
using Xunit;
using BidirectionalMap;

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
        public void OneToOneReversible()
        {
            // this would be a good use of fscheck
            BiMap<int, string> map = new BiMap<int, string>();
        }

        [Fact]
        public void ForwardAndReverseOperationallyConsistent()
        {

        }

        [Fact]
        public void InstantiateFromConstructor()
        {

        }

        [Fact]
        public void InstantiateFromInitializer()
        {

        }
    }
}

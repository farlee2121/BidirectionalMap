using System;
using Xunit;

namespace BidirectionalMap.Tests
{
    public class UnitTest1
    {
        // Tests
        // - make sure i can instantiate like a dictionary (given a set of keyvaluepairs)
        // - make sure I can instantiate throught the constructor with various types
        // - make sure I can add and remove while staying consistent (via kvp, add separate k and v, mutations of indexes)
        // - make sure contains always
        // - mmake sure it's 1-to-1 / reversible 
        // - test mapping to same value type
        // - 
        [Fact]
        public void OneToOneReversible()
        {
            BidirectionalMap<int, string> map = new BidirectionalMap<int, string>();
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

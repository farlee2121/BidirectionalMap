
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BidirectionalMap.Tests
{
    public class EqualityComparerTests
    {
        [Fact]
        public void ForwardMap_IgnoreCaseComparer_KeysAreTreatedAsEqual()
        {
            var initialDict = new Dictionary<string, int>
            {
                { "One", 1 }
            };
            var biMap = new BiMap<string, int>(initialDict, StringComparer.OrdinalIgnoreCase, null);

            Assert.True(biMap.Forward.ContainsKey("one"));
            Assert.Equal(1, biMap.Forward["ONE"]);
        }

        [Fact]
        public void ForwardMap_OrdinalComparer_KeysAreTreatedAsDistinct()
        {
            var initialDict = new Dictionary<string, int>
            {
                { "One", 1 }
            };
            var biMap = new BiMap<string, int>(initialDict, StringComparer.Ordinal, null);

            Assert.False(biMap.Forward.ContainsKey("one"));
            Assert.Equal(1, biMap.Forward["One"]);
        }

        [Fact]
        public void ForwardMap_CustomKeyComparer_KeysMatchh()
        {
            var initialDict = new Dictionary<string, int>
            {
                { "A", 1 }
            };
            var lengthComparer = new StringLengthComparer();
            var biMap = new BiMap<string, int>(initialDict, lengthComparer, null);

            Assert.True(biMap.Forward.ContainsKey("B"));
        }

        [Fact]
        public void ForwardMap_DefaultComparer_CanGetValue()
        {
            var initialDict = new Dictionary<string, int>
            {
                { "One", 1 }
            };
            var biMap = new BiMap<string, int>(initialDict, null, null);

            Assert.True(biMap.Forward.ContainsKey("One"));
            Assert.Equal(1, biMap.Forward["One"]);
        }

        [Fact]
        public void ReverseMap_IgnoreCaseComparer_KeysAreTreatedAsEqual()
        {
            var initialDict = new Dictionary<int, string>
            {
                { 1, "One" }
            };
            var biMap = new BiMap<int, string>(initialDict, null, StringComparer.OrdinalIgnoreCase);

            Assert.True(biMap.Reverse.ContainsKey("one"));
            Assert.Equal(1, biMap.Reverse["ONE"]);
        }

        [Fact]
        public void ReverseMap_OrdinalComparer_KeysAreTreatedAsDistinct()
        {
            var initialDict = new Dictionary<int, string>
            {
                { 1, "One" }
            };
            var biMap = new BiMap<int, string>(initialDict, null, StringComparer.Ordinal);

            Assert.False(biMap.Reverse.ContainsKey("one"));
            Assert.Equal(1, biMap.Reverse["One"]);
        }

        [Fact]
        public void ReverseMap_CustomKeyComparer_KeysMatchh()
        {
            var initialDict = new Dictionary<int, string>
            {
                { 1, "A" }
            };
            var lengthComparer = new StringLengthComparer();
            var biMap = new BiMap<int, string>(initialDict, null, lengthComparer);

            Assert.True(biMap.Reverse.ContainsKey("B"));
        }

        [Fact]
        public void ReverseMap_DefaultComparer_CanGetValue()
        {
            var initialDict = new Dictionary<string, int>
            {
                { "One", 1 }
            };
            var biMap = new BiMap<string, int>(initialDict, null, null);

            Assert.True(biMap.Reverse.ContainsKey(1));
            Assert.Equal("One", biMap.Reverse[1]);
        }

        [Fact]
        public void ForwardMap_CompareConstructor_IgnoreCaseComparer_KeysAreTreatedAsEqual()
        {
            var biMap = new BiMap<string, int>(StringComparer.OrdinalIgnoreCase, null);
            biMap.Add("One", 1);

            Assert.True(biMap.Forward.ContainsKey("one"));
            Assert.Equal(1, biMap.Forward["ONE"]);
        }

        [Fact]
        public void ForwardMap_CompareConstructor_OrdinalComparer_KeysAreTreatedAsDistinct()
        {
            var biMap = new BiMap<string, int>(StringComparer.Ordinal, null);
            biMap.Add("One", 1);

            Assert.False(biMap.Forward.ContainsKey("one"));
            Assert.Equal(1, biMap.Forward["One"]);
        }

        [Fact]
        public void ForwardMap_CompareConstructor_CustomKeyComparer_KeysMatchh()
        {
            var lengthComparer = new StringLengthComparer();
            var biMap = new BiMap<string, int>(lengthComparer, null);
            biMap.Add("A", 1);

            Assert.True(biMap.Forward.ContainsKey("B"));
        }

        [Fact]
        public void ForwardMap_CompareConstructor_DefaultComparer_CanGetValue()
        {
            var biMap = new BiMap<string, int>(StringComparer.Ordinal, null);
            biMap.Add("One", 1);

            Assert.True(biMap.Forward.ContainsKey("One"));
            Assert.Equal(1, biMap.Forward["One"]);
        }

        [Fact]
        public void ReverseMap_CompareConstructor_IgnoreCaseComparer_KeysAreTreatedAsEqual()
        {
            var biMap = new BiMap<int, string>(null, StringComparer.OrdinalIgnoreCase);
            biMap.Add(1, "One");

            Assert.True(biMap.Reverse.ContainsKey("one"));
            Assert.Equal(1, biMap.Reverse["ONE"]);
        }

        [Fact]
        public void ReverseMap_CompareConstructor_OrdinalComparer_KeysAreTreatedAsDistinct()
        {
            var biMap = new BiMap<int, string>(null, StringComparer.Ordinal);
            biMap.Add(1, "One");

            Assert.False(biMap.Reverse.ContainsKey("one"));
            Assert.Equal(1, biMap.Reverse["One"]);
        }

        [Fact]
        public void ReverseMap_CompareConstructor_CustomKeyComparer_KeysMatchh()
        {
            var lengthComparer = new StringLengthComparer();
            var biMap = new BiMap<int, string>(null, lengthComparer);
            biMap.Add(1, "A");

            Assert.True(biMap.Reverse.ContainsKey("B"));
        }

        [Fact]
        public void ReverseMap_CompareConstructor_DefaultComparer_CanGetValue()
        {
            var biMap = new BiMap<string, int>(null, null);
            biMap.Add("One", 1);

            Assert.True(biMap.Reverse.ContainsKey(1));
            Assert.Equal("One", biMap.Reverse[1]);
        }

        // Custom comparer for string length
        private class StringLengthComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y) => x?.Length == y?.Length;
            public int GetHashCode(string obj) => obj.Length.GetHashCode();
        }

    }
}

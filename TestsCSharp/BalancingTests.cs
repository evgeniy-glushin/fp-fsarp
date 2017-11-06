using NUnit.Framework;
using System.Collections.Generic;
using static Balancing;
using static CountChange;

namespace TestsCSharp
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Balancing()
        {
            Assert.True(balance("(if (zero? x) max (/ 1 x))"));
            Assert.True(balance("I told him (that it’s not (yet) done). (But he wasn’t listening)"));
            Assert.False(balance(":-)"));
            Assert.False(balance("())("));
        }

        [Test]
        public void CountChange()
        {
            Assert.AreEqual(expected: 3, actual: countChangeList(4, (new List<int> { 1, 2 })));
            Assert.AreEqual(expected: 9, actual: countChangeList(6, (new List<int> { 1, 2, 3, 4 })));
            Assert.AreEqual(expected: 7, actual: countChangeList(6, (new List<int> { 1, 2, 3 })));
            Assert.AreEqual(expected: 3, actual: countChangeList(4, (new List<int> { 1, 2 })));
            Assert.AreEqual(expected: 1, actual: countChangeList(6, (new List<int> { 6 })));
            Assert.AreEqual(expected: 1, actual: countChangeList(6, (new List<int> { 1 })));
        }
    }
}

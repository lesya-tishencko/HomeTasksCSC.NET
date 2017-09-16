using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trie;

namespace Trie.Tests
{
    [TestClass()]
    public class TrieTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var trie = new Trie();
            Assert.IsTrue(trie.Add("He"));
            Assert.IsTrue(trie.Add("She"));
            Assert.IsTrue(trie.Add("His"));
            Assert.IsTrue(trie.Add("Her"));
            Assert.IsFalse(trie.Add("He"));
            Assert.IsFalse(trie.Add(""));
            Assert.IsTrue(trie.Add("They"));
            Assert.IsFalse(trie.Add("They"));
            Assert.IsTrue(trie.Add("Their"));
        }

        [TestMethod()]
        public void ContainsTest()
        {
            var trie = new Trie();
            trie.Add("He");
            trie.Add("She");
            trie.Add("His");
            trie.Add("Her");
            trie.Add("They");
            trie.Add("Their");

            Assert.IsTrue(trie.Contains("He"));
            Assert.IsTrue(trie.Contains("She"));
            Assert.IsTrue(trie.Contains("His"));
            Assert.IsTrue(trie.Contains("Her"));
            Assert.IsFalse(trie.Contains("Here"));
            Assert.IsFalse(trie.Contains("H!s"));
            Assert.IsTrue(trie.Contains("They"));
            Assert.IsTrue(trie.Contains("Their"));

            trie.Remove("He");
            trie.Remove("They");
            trie.Remove("He");
            trie.Remove("Her");

            Assert.IsFalse(trie.Contains("He"));
            Assert.IsTrue(trie.Contains("She"));
            Assert.IsTrue(trie.Contains("His"));
            Assert.IsFalse(trie.Contains("Her"));
            Assert.IsFalse(trie.Contains("They"));
            Assert.IsTrue(trie.Contains("Their"));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var trie = new Trie();
            trie.Add("He");
            trie.Add("She");
            trie.Add("His");
            trie.Add("Her");
            trie.Add("Her");
            trie.Add("They");
            trie.Add("Their");

            Assert.IsTrue(trie.Remove("He"));
            Assert.IsFalse(trie.Remove("He"));
            Assert.IsFalse(trie.Remove("H!s"));
            Assert.IsTrue(trie.Remove("They"));
        }

        [TestMethod()]
        public void SizeTest()
        {
            var trie = new Trie();
            trie.Add("He");
            trie.Add("She");
            trie.Add("His");
            trie.Add("Her");
            trie.Add("Her");
            trie.Add("They");
            trie.Add("Their");

            Assert.AreEqual(6, trie.Size());

            trie.Remove("He");
            trie.Remove("They");
            trie.Remove("He");

            Assert.AreEqual(4, trie.Size());
        }

        [TestMethod()]
        public void HowManyStartsWithPrefixTest()
        {
            var trie = new Trie();
            trie.Add("He");
            trie.Add("Her");
            trie.Add("She");
            trie.Add("His");
            trie.Add("They");
            trie.Add("Their");

            Assert.AreEqual(2, trie.HowManyStartsWithPrefix("He"));
            Assert.AreEqual(3, trie.HowManyStartsWithPrefix("H"));
            Assert.AreEqual(2, trie.HowManyStartsWithPrefix("The"));

            trie.Remove("He");
            trie.Remove("They");
            trie.Remove("Her");

            Assert.AreEqual(0, trie.HowManyStartsWithPrefix("He"));
            Assert.AreEqual(1, trie.HowManyStartsWithPrefix("The"));
            Assert.AreEqual(1, trie.HowManyStartsWithPrefix("H"));

            trie.Add("Here");
            trie.Add("Her");

            Assert.AreEqual(2, trie.HowManyStartsWithPrefix("Her"));
        }
    }
}
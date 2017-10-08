using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Trie.Tests
{
    [TestClass()]
    public class TestTrie
    {
        private ITrie _trie;
        private List<string>[] _sourceLists = { new List<string> { "He", "She", "His", "Her", "They", "Their" } };


        [TestInitialize()]
        public void Initialize()
        {
            _trie = new Trie();
        }

        [TestMethod()]
        public void TestAdd()
        {
            _sourceLists[0].ForEach(elem => Assert.IsTrue(_trie.Add(elem)));
            Assert.IsFalse(_trie.Add("He"));
            Assert.IsFalse(_trie.Add(""));
            Assert.IsFalse(_trie.Add("They"));
        }

        [TestMethod()]
        public void TestContains()
        {
            _sourceLists[0].ForEach(elem => _trie.Add(elem));
            _sourceLists[0].ForEach(elem => Assert.IsTrue(_trie.Contains(elem)));

            Assert.IsFalse(_trie.Contains("Here"));
            Assert.IsFalse(_trie.Contains("H!s"));

            _trie.Remove("He");
            _trie.Remove("They");
            _trie.Remove("He");
            _trie.Remove("Her");

            Assert.IsFalse(_trie.Contains("He"));
            Assert.IsTrue(_trie.Contains("She"));
            Assert.IsTrue(_trie.Contains("His"));
            Assert.IsFalse(_trie.Contains("Her"));
            Assert.IsFalse(_trie.Contains("They"));
            Assert.IsTrue(_trie.Contains("Their"));
        }

        [TestMethod()]
        public void TestRemove()
        {
            _sourceLists[0].ForEach(elem => _trie.Add(elem));

            Assert.IsTrue(_trie.Remove("He"));
            Assert.IsFalse(_trie.Remove("He"));
            Assert.IsFalse(_trie.Remove("H!s"));
            Assert.IsTrue(_trie.Remove("They"));
        }

        [TestMethod()]
        public void TestSize()
        {
            _sourceLists[0].ForEach(elem => _trie.Add(elem));

            Assert.AreEqual(6, _trie.Size());

            _trie.Remove("He");
            _trie.Remove("They");

            Assert.AreEqual(4, _trie.Size());
        }

        [TestMethod()]
        public void TestHowManyStartsWithPrefix()
        {
            _sourceLists[0].ForEach(elem => _trie.Add(elem));

            Assert.AreEqual(2, _trie.HowManyStartsWithPrefix("He"));
            Assert.AreEqual(3, _trie.HowManyStartsWithPrefix("H"));
            Assert.AreEqual(2, _trie.HowManyStartsWithPrefix("The"));

            _trie.Remove("He");
            _trie.Remove("They");
            _trie.Remove("Her");

            Assert.AreEqual(0, _trie.HowManyStartsWithPrefix("He"));
            Assert.AreEqual(1, _trie.HowManyStartsWithPrefix("The"));
            Assert.AreEqual(1, _trie.HowManyStartsWithPrefix("H"));

            _trie.Add("Here");
            _trie.Add("Her");

            Assert.AreEqual(2, _trie.HowManyStartsWithPrefix("Her"));
        }

        [TestMethod()]
        public void TestIndexOutOfRangeException()
        {
            Assert.IsTrue(_trie.Add("Привет!"));
            Assert.IsTrue(_trie.Add("Пока!"));
            Assert.IsTrue(_trie.Remove("Привет!"));
        }
    }
}
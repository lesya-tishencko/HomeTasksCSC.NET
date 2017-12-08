using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OptionTask.Tests
{
    [TestClass]
    public class TestOption
    {
        [TestMethod]
        public void TestSomeInt()
        {
            Assert.AreEqual(100, Option.Some(100).Value());
        }

        [TestMethod]
        public void TestSomeChar()
        {
            Assert.AreEqual('4', Option.Some('4').Value());
        }

        [TestMethod]
        public void TestSomeString()
        {
            Assert.IsTrue(Option.Some("Hello").IsSome);
            string variable = null;
            Assert.IsNull(Option.Some(variable).Value());
        }

        [TestMethod]
        public void TestSomeDouble()
        {
            Assert.IsFalse(Option.Some(0.0001).IsNone);
        }

        [TestMethod]
        [ExpectedException(typeof(OptionException))]
        public void TestNoneWithException()
        {
            Option.None<int>().Value();
        }

        [TestMethod]
        public void TestNone()
        {
            Assert.IsTrue(Option.None<int>().IsNone);
            Assert.IsFalse(Option.None<int>().IsSome);
        }

        [TestMethod]
        public void TestMapSome()
        {
            Assert.AreEqual(Option.Some(4), Option.Some(2).Map(x => x * 2));
            Assert.AreEqual(Option.Some("40"), Option.Some(40).Map(x => x.ToString()));
        }

        [TestMethod]
        public void TestMapNone()
        {
            Assert.AreEqual(Option.None<int>(), Option.None<int>().Map(x => x * 2));
            Assert.AreEqual(Option.None<string>(), Option.None<int>().Map(x => x.ToString()));
        }

        [TestMethod]
        public void TestFlattenSome()
        {
            Assert.AreEqual(Option.Some(4), Option.Some(Option.Some(4)).Flatten());
            Assert.AreEqual(4, Option.Some(Option.Some(4)).Flatten().Value());
        }

        [TestMethod]
        public void TestFlattenNone()
        {
            Assert.AreEqual(Option.None<int>(), Option.Some(Option.None<int>()).Flatten());
            Assert.IsTrue(Option.Some(Option.None<int>()).Flatten().IsNone);
        }

        [TestMethod]
        public void TestNoneReferenceNone()
        {
            Assert.AreEqual(Option.None<int>(), Option.None<int>());
        }
    }
}
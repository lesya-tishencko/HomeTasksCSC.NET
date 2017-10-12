using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OptionTask.Tests
{
    [TestClass()]
    public class TestOption
    {
        [TestMethod()]
        public void TestSome()
        {
            Assert.AreEqual(100, Option.Some(100).Value());
            Assert.AreEqual('4', Option.Some('4').Value());
            Assert.IsTrue(Option.Some("Hello").IsSome());
            Assert.IsFalse(Option.Some(0.0001).IsNone());
            string variable = null;
            Assert.IsNull(Option.Some(variable).Value());
        }

        [TestMethod()]
        [ExpectedException(typeof(OptionException))]
        public void TestNone()
        {
            Assert.IsTrue(Option.None<int>().IsNone());
            Assert.IsFalse(Option.None<int>().IsSome());
            Option.None<int>().Value();
        }

        [TestMethod()]
        public void TestMap()
        {
            Assert.AreEqual(Option.Some(4), Option.Some(2).Map(x => x * 2));
            Assert.AreEqual(Option.None<int>(), Option.None<int>().Map(x => x * 2));
            Assert.AreEqual(Option.Some("40"), Option.Some(40).Map(x => x.ToString()));
            Assert.AreEqual(Option.None<string>(), Option.None<int>().Map(x => x.ToString()));
        }

        [TestMethod()]
        public void TestFlatten()
        {
            Assert.AreEqual(Option.Some(4), Option.Flatten(Option.Some(Option.Some(4))));
            Assert.AreEqual(Option.None<int>(), Option.Flatten(Option.Some(Option.None<int>())));
            Assert.IsTrue(Option.Flatten(Option.Some(Option.None<int>())).IsNone());
            Assert.AreEqual(4, Option.Flatten(Option.Some(Option.Some(4))).Value());
        }
    }
}
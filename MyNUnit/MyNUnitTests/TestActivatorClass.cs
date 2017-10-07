using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyNUnit.Tests
{
    [TestClass()]
    public class TestActivatorClass
    {
        [TestMethod()]
        public void TestAssembliesPath()
        {
            var testHandler = new TestActivator(@"../../..");
            Assert.AreEqual(13, testHandler.assemblies.Length);

            testHandler = new TestActivator(@"../../../NUnit/bin/Debug");
            Assert.AreEqual(1, testHandler.assemblies.Length);

            testHandler = new TestActivator(@"../../../NUnitTest/bin/Debug");
            Assert.AreEqual(2, testHandler.assemblies.Length);
        }

        [TestMethod()]
        public void TestRunningWithoutException()
        {
            var testHandler = new TestActivator(@"../../..");
            testHandler.LoadTests();
        }
    }
}

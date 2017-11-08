using System;
using static NUnit.NUnitAssert;

namespace NUnit
{
    public class ExampleTestCase
    {
        private int x, y = -1;

        [Before]
        public void BeforeTest()
        {
            x = 1;
        }

        [After]
        public void AfterTest()
        {
            y = 0;
            Assert(y == 0, "y == 0");
        }

        [Test]
        // Must be crashed
        public void TestMethod1()
        {
            Assert((x += 1) == 2, "x += 1 == 2");
            Assert(x + 1 == 2, "x + 1 == 2");
        }

        [Test(ExpectedException = typeof(DivideByZeroException))]
        public void TestMethod2()
        {
            Assert((x /= 0) == 0, "x /= 0");
        }

        [Test(Ignore = "Test ignore attribute")]
        public void TestMethod3()
        {

        }
    }
}

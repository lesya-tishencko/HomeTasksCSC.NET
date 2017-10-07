using NUnit;
using static NUnit.NUnitAssert;

namespace NUnitTest
{
    public class TestBeforeMethodFailedExample
    {
        [Before]
        public void BeforeMethod()
        {
            Assert(1 == 0, "Just checking");
        }

        [Test]
        public void TestMethod()
        {

        }
    }
}

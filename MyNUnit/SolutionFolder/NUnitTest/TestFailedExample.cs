using NUnit;
using static NUnit.NUnitAssert;

namespace NUnitTest
{
    public class TestFailedExample
    {
        private int[] array = new int[1];

        [BeforeClass]
        public void BeforeTestClassCrashed()
        {
            Assert(array.Length == 0, "array.size == 0");
        }
    }
}

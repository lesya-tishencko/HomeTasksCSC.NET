using System;
using static NUnit.NUnitAssert;

namespace NUnit
{
    public class OneMoreTestCaseExample
    {
        private int[] array;

        [BeforeClass]
        public void BeforeTestClass()
        {
            array = new int[5];
        }

        [Test]
        // Must be crashed
        public void TestMethod1()
        {
            throw new DivideByZeroException();
        }

        [Test(ExpectedException = typeof(IndexOutOfRangeException))]
        public void TestMethod2()
        {
            array[5] = 2;
        }

        [Test(Ignore = "Because I can")]
        public void TestMethod3()
        {
            array[5] = 0;
        }

        [AfterClass]
        public void AfterTestClass()
        {
            array = new int[1];
            Assert(array.Length == 1, "array.size == 1");
        }

        [Test] 
        public void TestMethod4()
        {
            for (int i = 0; i < 5; i++)
            {
                array[i] = i;
            }
        }
    }
}

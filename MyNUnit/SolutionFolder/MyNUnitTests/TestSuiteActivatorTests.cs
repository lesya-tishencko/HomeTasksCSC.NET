using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace MyNUnit.Tests
{
    [TestClass]
    public class TestSuiteActivatorClass
    {
        private Type[] typesNUnit, typesNUnitTest;

        [TestInitialize]
        public void Initialize()
        {
            var assemblyNUnit = Assembly.LoadFrom(@"../../../NUnit/bin/Debug/NUnit.dll");
            typesNUnit = assemblyNUnit.GetExportedTypes();
            Assert.AreEqual(9, typesNUnit.Length);

            var assemblyNUnitTest = Assembly.LoadFrom(@"../../../NUnitTest/bin/Debug/NUnitTest.dll");
            typesNUnitTest = assemblyNUnitTest.GetExportedTypes();
            Assert.AreEqual(2, typesNUnitTest.Length);
        }

        
        [TestMethod]
        public void TestExampleTestCase()
        {
            var testType = typesNUnit[4];
            var testHandler = new TestSuiteActivator(testType);

            testHandler.beforeClassMethods.ForEach(meth => meth.Invoke(testHandler.obj, null));

            string result = "Fail: Test ExampleTestCase.TestMethod1 crashed with exception:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod1")).StartsWith(result));

            result = "Success: Test ExampleTestCase.TestMethod2 crashed with expected exception:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod2")).StartsWith(result));

            result = "Success: Test ExampleTestCase.TestMethod3 was ignored by the reason:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod3")).StartsWith(result));

            testHandler.afterClassMethods.ForEach(meth => meth.Invoke(testHandler.obj, null));
        }

        [TestMethod]
        public void TestOneMoreTestCaseExample()
        {
            var testType = typesNUnit[7];
            var testHandler = new TestSuiteActivator(testType);
            testHandler.beforeClassMethods.ForEach(meth => meth.Invoke(testHandler.obj, null));

            string result = "Fail: Test OneMoreTestCaseExample.TestMethod1 crashed with exception:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod1")).StartsWith(result));

            result = "Success: Test OneMoreTestCaseExample.TestMethod2 crashed with expected exception:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod2")).StartsWith(result));

            result = "Success: Test OneMoreTestCaseExample.TestMethod3 was ignored by the reason:";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod3")).StartsWith(result));

            result = "Success: Test OneMoreTestCaseExample.TestMethod4 passed";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod4")).StartsWith(result));

            testHandler.afterClassMethods.ForEach(meth => meth.Invoke(testHandler.obj, null));
        }

        [TestMethod]
        public void TestBeforeMethodFailedExample()
        {
            var testType = typesNUnitTest[0];
            var testHandler = new TestSuiteActivator(testType);

            string result = "Before test methods in TestBeforeMethodFailedExample throws";
            Assert.IsTrue(testHandler.RunTestMethod(testType.GetMethod("TestMethod")).StartsWith(result));
        }

        [TestMethod]
        public void TestFailedExample()
        {
            var testHandler = new TestSuiteActivator(typesNUnitTest[1]);
            string result = "Test suit: TestFailedExample throws";
            Assert.IsTrue(testHandler.RunTests()[0].StartsWith(result));
        }
    }
}
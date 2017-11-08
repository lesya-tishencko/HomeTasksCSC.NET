using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyNUnitTests")]
namespace MyNUnit
{
    public class TestSuiteActivator
    {
        private Type type;
        internal object obj;
        private List<MethodInfo> testMethods = new List<MethodInfo>();
        internal List<MethodInfo> beforeClassMethods = new List<MethodInfo>();
        internal List<MethodInfo> afterClassMethods = new List<MethodInfo>();
        private List<MethodInfo> beforeMethods = new List<MethodInfo>();
        private List<MethodInfo> afterMethods = new List<MethodInfo>();
        private static Stopwatch stopWatch = new Stopwatch();

        public TestSuiteActivator(Type type)
        {
            this.type = type;
            var ctor = type.GetTypeInfo().GetConstructor(Type.EmptyTypes);
            if (ctor != null && !ctor.ContainsGenericParameters)
            {
                obj = ctor.Invoke(null);
            }

            foreach (var methInfo in type.GetMethods().Where(meth => !meth.IsSecurityCritical))
            {
                var attributes = CustomAttributeData.GetCustomAttributes(methInfo);

                if (IsAfterClassMethod(attributes)) afterClassMethods.Add(methInfo);
                else if (IsAfterMethod(attributes)) afterMethods.Add(methInfo);
                else if (IsBeforeClassMethod(attributes)) beforeClassMethods.Add(methInfo);
                else if (IsBeforeMethod(attributes)) beforeMethods.Add(methInfo);
                else if (IsTestMethod(attributes)) testMethods.Add(methInfo);
            }
        }

        public string[] RunTests()
        {
            try
            {
                beforeClassMethods.ToObservable().Subscribe(method => method.Invoke(obj, null));
                var result = testMethods.Select(RunTestMethod).ToArray();
                afterClassMethods.ToObservable().Subscribe(method => method.Invoke(obj, null));
                return result;
            }
            catch (Exception exc)
            {
                return new[] { $"Test suit: {type.Name} throws unhandled exception: {exc.InnerException?.Message}" };
            }
        }

        internal string RunTestMethod(MethodInfo test)
        {
            string status;
            try
            {
                beforeMethods.ToObservable().Subscribe(method => method.Invoke(obj, null));
            }
            catch (Exception exc)
            {
                return $"Before test methods in {type.Name} throws unhandled exception: {exc.InnerException.Message}";
            }
            
            var attr = CustomAttributeData
                                      .GetCustomAttributes(test)
                                      .First(at => at.AttributeType.Name == "Test");
            Type expectedException = null;
            string reason = null;
            foreach (var cana in attr.NamedArguments.Where(arg => arg != null))
            {
                if (cana.MemberName == "ExpectedException") expectedException = (Type)cana.TypedValue.Value;
                if (cana.MemberName == "Ignore") reason = (string)cana.TypedValue.Value;
            }

            if (reason != null)
            {
                status = "Success: Test " + type.Name + "." + test.Name + " was ignored by the reason: " + reason;
                return status;
            }

            try
            {
                stopWatch.Start();
                test.Invoke(obj, null);
                status = "Success: Test " + type.Name + "." + test.Name + " passed";
            }
            catch (Exception exc) 
            {
                if (expectedException != null && exc.InnerException.GetType() == expectedException)
                {
                    status = "Success: Test " + type.Name + "." + test.Name + " crashed with expected exception: " + expectedException;
                }
                else
                {
                    status = "Fail: Test " + type.Name + "." + test.Name + " crashed with exception: " + exc.InnerException.Message;
                }
                 
            }
            finally
            {
                stopWatch.Stop();
            }
            if (expectedException != null && status.Contains("passed"))
            {
                status = "Fail: Test " + type.Name + "." + test.Name + " didn't catch " + expectedException;
            }

            var ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            status += Environment.NewLine + elapsedTime;

            try
            {
                afterMethods.ToObservable().Subscribe(method => method.Invoke(obj, null));
            }
            catch (Exception exc)
            {
                return $"After test methods in {type.Name} throws unhandles exception: {exc.InnerException.Message}";
            }
            return status;
        }
        
        private bool IsTestMethod(IList<CustomAttributeData> attributes)
            => attributes.Any(attr => attr.AttributeType.Name == "Test");

        private bool IsBeforeClassMethod(IList<CustomAttributeData> attributes)
            => attributes.Any(attr => attr.AttributeType.Name == "BeforeClass");

        private bool IsAfterClassMethod(IList<CustomAttributeData> attributes)
            => attributes.Any(attr => attr.AttributeType.Name == "AfterClass");

        private bool IsBeforeMethod(IList<CustomAttributeData> attributes)
            => attributes.Any(attr => attr.AttributeType.Name == "Before");

        private bool IsAfterMethod(IList<CustomAttributeData> attributes)
            => attributes.Any(attr => attr.AttributeType.Name == "After");
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MyNUnit
{
    public class TestSuiteActivator
    {
        private Type type;
        private object obj;
        private List<MethodInfo> testMethods = new List<MethodInfo>();
        private List<MethodInfo> beforeClassMethods = new List<MethodInfo>();
        private List<MethodInfo> afterClassMethods = new List<MethodInfo>();
        private List<MethodInfo> beforeMethods = new List<MethodInfo>();
        private List<MethodInfo> afterMethods = new List<MethodInfo>();
        private static Stopwatch stopWatch = new Stopwatch();

        public TestSuiteActivator(Type type)
        {
            this.type = type;
            ConstructorInfo ctor = type.GetTypeInfo().GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                obj = ctor.Invoke(null);
            }
        }

        public string[] RunTests()
        {
            var methods = type.GetMethods();
            foreach (var methInfo in methods)
            {
                if (IsTestMethod(methInfo)) testMethods.Add(methInfo);
                else if (IsBeforeClassMethod(methInfo)) beforeClassMethods.Add(methInfo);
                else if (IsAfterClassMethod(methInfo)) afterClassMethods.Add(methInfo);
                else if (IsBeforeMethod(methInfo)) beforeMethods.Add(methInfo);
                else if (IsAfterMethod(methInfo)) afterMethods.Add(methInfo);
            }

            try
            {
                beforeClassMethods.ForEach(method => method.Invoke(obj, null));
                var result = testMethods.Select(test => RunTestMethod(test)).ToArray();
                afterClassMethods.ForEach(method => method.Invoke(obj, null));
                return result;
            }
            catch (Exception exc)
            {
                return new string[]{ $"Test suit: {type.Name} methods throws unhandled exception: {exc.InnerException.Message}" };
            }
        }

        private string RunTestMethod(MethodInfo test)
        {
            string status;
            try
            {
                beforeMethods.ForEach(method => method.Invoke(obj, null));
            } catch (Exception exc)
            {
                return $"Before test methods in {type.Name} throws unhandled exception: {exc.InnerException.Message}";
            }

            var attr = CustomAttributeData.GetCustomAttributes(test)
                                          .Where(at => at.AttributeType.Name == "Test")
                                          .First();
            Type expectedException = null;
            string reason = null;
            foreach (CustomAttributeNamedArgument cana in attr.NamedArguments)
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
                    status = "Success: Test " + test.DeclaringType.FullName + "." + test.Name + " crashed with expected exception: " + expectedException.ToString();
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

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            status += '\n' + elapsedTime;

            try {
                afterMethods.ForEach(method => method.Invoke(obj, null));
            }
            catch (Exception exc)
            {
                return $"After test methods in {type.Name} throws unhandles exception: {exc.InnerException.Message}";
            }
            return status;
        }
        
        private bool IsTestMethod(MethodInfo method)
            => CustomAttributeData.GetCustomAttributes(method)
                                  .Where(attr => attr.AttributeType.Name == "Test")
                                  .Count() > 0;

        private bool IsBeforeClassMethod(MethodInfo method)
            => CustomAttributeData.GetCustomAttributes(method)
                                  .Where(attr => attr.AttributeType.Name == "BeforeClass")
                                  .Count() > 0;

        private bool IsAfterClassMethod(MethodInfo method)
            => CustomAttributeData.GetCustomAttributes(method)
                                  .Where(attr => attr.AttributeType.Name == "AfterClass")
                                  .Count() > 0;

        private bool IsBeforeMethod(MethodInfo method)
            => CustomAttributeData.GetCustomAttributes(method)
                                  .Where(attr => attr.AttributeType.Name == "Before")
                                  .Count() > 0;

        private bool IsAfterMethod(MethodInfo method)
            => CustomAttributeData.GetCustomAttributes(method)
                                  .Where(attr => attr.AttributeType.Name == "After")
                                  .Count() > 0;
    }
}

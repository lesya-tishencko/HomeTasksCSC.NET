using System;

namespace NUnit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Test : Attribute
    {
        public Test() { }
        public Type ExpectedException { get; set; }
        public string Ignore { get; set; }
    }
}
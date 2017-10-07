using System;

namespace NUnit
{
    public class MyNUnitAssertException : Exception
    {
        public MyNUnitAssertException(string message) : base(message) { }
    }
}

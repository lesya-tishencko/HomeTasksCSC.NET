using System;

namespace NUnit
{
    [Serializable]
    public class MyNUnitAssertException : Exception
    {
        public MyNUnitAssertException() { }
        public MyNUnitAssertException(string message) : base(message) { }
        public MyNUnitAssertException(string message, Exception inner) : base(message, inner) { }
        protected MyNUnitAssertException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

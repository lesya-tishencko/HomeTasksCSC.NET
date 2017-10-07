namespace NUnit
{
    public static class NUnitAssert
    {
        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        public static void Fail(string message)
        {
            throw new MyNUnitAssertException($"Assertion failed: {message}");
        }
    }
}

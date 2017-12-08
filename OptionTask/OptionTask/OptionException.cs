using System;

namespace OptionTask
{
    public class OptionException: Exception {
        public OptionException() { }
        public OptionException(string message): base(message) { }
    }
}

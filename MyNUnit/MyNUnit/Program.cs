namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            var tester = new TestActivator(@"../../..");
            tester.LoadTests();
        }
    }
}

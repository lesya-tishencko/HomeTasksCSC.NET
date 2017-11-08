using System.Reflection;
using System.Linq;
using System.Reactive.Linq;
using System;
using System.IO;

namespace MyNUnit
{
    public class TestActivator
    {
        internal Assembly[] assemblies;

        public TestActivator(string path) 
        {
            assemblies = Directory
                .GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(file => new string[] { ".dll", ".exe" }
                .Contains(Path.GetExtension(file)))
                .Select(Assembly.LoadFrom)
                .ToArray();
        }

        public void LoadTests()
        {
            assemblies
                .SelectMany(asmb => asmb.GetExportedTypes())
                .Distinct()
                .Select(type => new TestSuiteActivator(type))
                .Select(suite => suite.RunTests())
                .ToObservable()
                .Subscribe(PrintReport);
        }

        private void PrintReport(string[] lines)
        {
            lines
                .ToObservable()
                .Subscribe(Console.WriteLine);
            if (lines.Length > 0)
            {
                Console.WriteLine();
            }
        }

    }
}

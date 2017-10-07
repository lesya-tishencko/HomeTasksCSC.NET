using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace MyNUnit
{
    public class TestActivator
    {
        private Assembly[] assemblies;

        public TestActivator(string path) 
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                                      .Where(file => new string[] { ".dll", ".exe" }
                                      .Contains(Path.GetExtension(file)))
                                      .ToArray();
            assemblies = files.Select(file => Assembly.LoadFrom(file)).ToArray();
        }

        public void LoadTests()
        {
            var report = assemblies.SelectMany(asmb => asmb.GetExportedTypes())
                                   .Distinct()
                                   .Select(type => new TestSuiteActivator(type))
                                   .Select(suite => suite.RunTests());
            report.ToList().ForEach(array => PrintReport(array));
        }

        private void PrintReport(string[] lines)
        {
            lines.ToList().ForEach(line => Console.WriteLine(line));
            if (lines.Length > 0)
            {
                Console.WriteLine();
            }
        }

    }
}

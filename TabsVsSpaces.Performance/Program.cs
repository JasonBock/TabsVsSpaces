using BenchmarkDotNet.Running;
using System;

namespace TabsVsSpaces.Performance
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Out.WriteLine(
				BenchmarkRunner.Run<BuildWithWorkspaceTests>());

			//var tests = new BuildWithWorkspaceTests();
			//Console.Out.WriteLine($"Results: {tests.BuildNodaTimeWithSpaces().Result.Count}");
			//Console.Out.WriteLine($"Results: {tests.BuildNodaTimeWithTabs().Result.Count}");

			//Console.Out.WriteLine(
			//	BenchmarkRunner.Run<BuildWithMSBuildProcessTests>());

			//var tests = new BuildWithMSBuildProcessTests();
			//var exitCode = tests.BuildCslaWithSpaces();
			//Console.Out.WriteLine($"Results: {exitCode}");
		}
	}
}

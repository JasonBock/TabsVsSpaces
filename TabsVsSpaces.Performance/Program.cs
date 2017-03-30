using BenchmarkDotNet.Running;
using System;

namespace TabsVsSpaces.Performance
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Out.WriteLine(
				BenchmarkRunner.Run<BuildTests>());

			//var tests = new BuildTests();
			//var results = tests.BuildRocksWithSpaces();
			//results.Wait();
			//Console.Out.WriteLine($"Results: {results.Result.Count}");
		}
	}
}

using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp;
using System;

namespace TabsVsSpaces.Performance
{
	class Program
	{
		static void Main(string[] args)
		{
			//Console.Out.WriteLine(
			//	BenchmarkRunner.Run<BuildWithWorkspaceTests>());

			//var tests = new BuildWithWorkspaceTests();
			//Console.Out.WriteLine($"Results: {tests.BuildNodaTimeWithSpaces().Result.Count}");
			//Console.Out.WriteLine($"Results: {tests.BuildNodaTimeWithTabs().Result.Count}");

			//Console.Out.WriteLine(
			//	BenchmarkRunner.Run<BuildWithMSBuildProcessTests>());

			//var tests = new BuildWithMSBuildProcessTests();
			//var exitCode = tests.BuildCslaWithSpaces();
			//Console.Out.WriteLine($"Results: {exitCode}");

			//Console.Out.WriteLine(
			//	BenchmarkRunner.Run<BuildWithSyntaxFactoryTests>());

			//var tests = new BuildWithSyntaxFactoryTests();
			//Console.Out.WriteLine($"Spaces results: {tests.BuildRocksWithSpacesAsync().Result}");
			//Console.Out.WriteLine($"Tabs results: {tests.BuildRocksWithTabsAsync().Result}");

			//Console.Out.WriteLine(
			//	BenchmarkRunner.Run<ParseExpressionTests>());

			var tests = new ParseExpressionTests();
			var expressionSpaces = tests.ParseWithSpaces();

			var whitespaceSpaces = expressionSpaces.DescendantTrivia(_ => true, true);

			foreach(var whitespace in whitespaceSpaces)
			{
				Console.Out.WriteLine(whitespace);
			}

			var expressionTabs = tests.ParseWithSpaces();

			var whitespaceTabs = expressionTabs.DescendantTrivia(_ => true, true);

			foreach (var whitespaceT in whitespaceTabs)
			{
				Console.Out.WriteLine(whitespaceT);
			}
		}
	}
}

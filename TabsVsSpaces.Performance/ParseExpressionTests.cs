using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TabsVsSpaces.Performance
{
	public class ParseExpressionTests
	{
		[Benchmark]
		public ExpressionSyntax ParseWithTabs()
		{
			return SyntaxFactory.ParseExpression("		myObject != null");
		}

		[Benchmark]
		public ExpressionSyntax ParseWithSpaces()
		{
			return SyntaxFactory.ParseExpression("      myObject != null");
		}
	}
}

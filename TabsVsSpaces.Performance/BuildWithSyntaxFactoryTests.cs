using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;
using System.Threading.Tasks;

namespace TabsVsSpaces.Performance
{
	public class BuildWithSyntaxFactoryTests
	{
		//[Benchmark]
		//public async Task<int> BuildRocksWithTabsAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Rocks\tab\Rocks.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildRocksWithSpacesAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Rocks\space\Rocks.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAutofacWithTabsAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Autofac\tab\Autofac.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAutofacWithSpacesAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Autofac\space\Autofac.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAutoMapperWithTabsAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\AutoMapper\tab\src\AutoMapper.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAutoMapperWithSpacesAsync()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\AutoMapper\space\src\AutoMapper.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildMoqWithTabs()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Moq\tab\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildMoqWithSpaces()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Moq\space\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNewtonsoftJsonWithTabs()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Newtonsoft.Json\tab\Src\Newtonsoft.Json.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNewtonsofJsonWithSpaces()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Newtonsoft.Json\space\Src\Newtonsoft.Json.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAngleSharpWithTabs()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\AngleSharp\tab\src\AngleSharp.Core.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildAngleSharpWithSpaces()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\AngleSharp\space\src\AngleSharp.Core.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNLogWithTabs()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\NLog\tab\src\NLog.netfx45.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNLogWithSpaces()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\NLog\space\src\NLog.netfx45.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNodaTimeWithTabs()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\NodaTime\tab\src\NodaTime-All.sln");
		//}

		//[Benchmark]
		//public async Task<int> BuildNodaTimeWithSpaces()
		//{
		//	return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\NodaTime\space\src\NodaTime-All.sln");
		//}

		[Benchmark]
		public async Task<int> BuildCslaWithTabs()
		{
			return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Csla\tab\Source\csla.net.sln");
		}

		[Benchmark]
		public async Task<int> BuildCslaWithSpaces()
		{
			return await BuildWithSyntaxFactoryTests.RunCompilationAsync(@"M:\TVSSource\Repos\Csla\space\Source\csla.net.sln");
		}

		private static async Task<int> RunCompilationAsync(string solutionFile)
		{
			var files = 0;
			var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(solutionFile);

			foreach (var project in solution.Projects)
			{
				foreach (var document in project.Documents)
				{
					if(Path.GetExtension(document.FilePath).ToLower() == ".cs")
					{
						SyntaxFactory.ParseCompilationUnit(File.ReadAllText(document.FilePath));
						files++;
					}
				}
			}

			return files;
		}
	}
}
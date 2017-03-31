using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TabsVsSpaces.Performance
{
	public class BuildWithWorkspaceTests
	{
		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildCslaWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Csla\tab\Source\csla.net.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildCslaWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Csla\space\Source\csla.net.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildRocksWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\tab\Rocks.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildRocksWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\space\Rocks.sln");
		//}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildAutofacWithTabs()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\tab\Autofac.sln",
				new[] { "Autofac.Test.Uwp.DeviceRunner", "Autofac.Benchmarks" });
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildAutofacWithSpaces()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\space\Autofac.sln",
				new[] { "Autofac.Test.Uwp.DeviceRunner", "Autofac.Benchmarks" });
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildAutoMapperWithTabs()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\tab\src\AutoMapper.sln",
				new[] { "AutoMapper.UnitTests.Net4", "AutoMapperSamples", "Benchmark", "AutoMapper.IntegrationTests.Net4", "AutoMapperSamples.EF", "AutoMapperSamples.OData" });
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildAutoMapperWithSpaces()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\space\src\AutoMapper.sln",
				new[] { "AutoMapper.UnitTests.Net4", "AutoMapperSamples", "Benchmark", "AutoMapper.IntegrationTests.Net4", "AutoMapperSamples.EF", "AutoMapperSamples.OData" });
		}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildMoqWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Moq\tab\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildMoqWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Moq\space\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNewtonsoftJsonWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Newtonsoft.Json\tab\Src\Newtonsoft.Json.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNewtonsofJsonWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Newtonsoft.Json\space\Src\Newtonsoft.Json.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAngleSharpWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\tab\src\AngleSharp.Core.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAngleSharpWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\space\src\AngleSharp.Core.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNLogWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\NLog\tab\src\NLog.netfx45.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNLogWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\NLog\space\src\NLog.netfx45.sln");
		//}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildNodaTimeWithTabs()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\tab\src\NodaTime-All.sln",
				new[] { "NodaTime.Benchmarks", "NodaTime.Demo", "NodaTime.Test", "NodaTime.TzdbCompiler", "NodaTime.TzdbCompiler.Test", "NodaTime.TzValidate.NodaDump", "NodaTime.NzdPrinter" });
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildNodaTimeWithSpaces()
		{
			return await BuildWithWorkspaceTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\space\src\NodaTime-All.sln",
				new[] { "NodaTime.Benchmarks", "NodaTime.Demo", "NodaTime.Test", "NodaTime.TzdbCompiler", "NodaTime.TzdbCompiler.Test", "NodaTime.TzValidate.NodaDump", "NodaTime.NzdPrinter" });
		}

		private static async Task<ImmutableList<EmitResult>> RunCompilation(
			string solutionFile, string[] projectsToIgnore)
		{
			CSharpCompilationOptions CreateOptions(CompilationOptions options)
			{
				return new CSharpCompilationOptions(options.OutputKind,
					reportSuppressedDiagnostics: options.ReportSuppressedDiagnostics,
					moduleName: options.ModuleName,
					mainTypeName: options.MainTypeName,
					scriptClassName: options.ScriptClassName,
					optimizationLevel: OptimizationLevel.Release,
					checkOverflow: options.CheckOverflow,
					allowUnsafe: (options as CSharpCompilationOptions)?.AllowUnsafe ?? true,
					cryptoKeyContainer: options.CryptoKeyContainer,
					cryptoKeyFile: options.CryptoKeyFile,
					cryptoPublicKey: options.CryptoPublicKey,
					delaySign: options.DelaySign,
					platform: options.Platform,
					generalDiagnosticOption: options.GeneralDiagnosticOption,
					warningLevel: options.WarningLevel,
					specificDiagnosticOptions: options.SpecificDiagnosticOptions,
					concurrentBuild: options.ConcurrentBuild,
					deterministic: options.Deterministic,
					xmlReferenceResolver: options.XmlReferenceResolver,
					sourceReferenceResolver: options.SourceReferenceResolver,
					metadataReferenceResolver: options.MetadataReferenceResolver,
					assemblyIdentityComparer: options.AssemblyIdentityComparer,
					strongNameProvider: options.StrongNameProvider,
					publicSign: options.PublicSign);
			}

			var results = new List<EmitResult>();
			var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(solutionFile);

			foreach (var projectId in solution.GetProjectDependencyGraph().GetTopologicallySortedProjects())
			{
				var compilation = await solution.GetProject(projectId).GetCompilationAsync();

				if(!projectsToIgnore.Contains(compilation.AssemblyName))
				{
					using (var stream = new MemoryStream())
					{
						compilation = compilation.WithOptions(CreateOptions(compilation.Options));
						var emitResult = compilation.Emit(stream);

						if (!emitResult.Success)
						{
							throw new WorkspaceBuildException(emitResult.Diagnostics);
						}
						else
						{
							results.Add(emitResult);
						}
					}
				}
			}

			return results.ToImmutableList();
		}
	}
}
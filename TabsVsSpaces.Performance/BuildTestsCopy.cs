// This file is here purely as backup for the "real" BuildTests class.
// It isn't built with the project and does not come along as output.
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace TabsVsSpaces.Performance
{
	public class BuildTests
	{
		public static void Run()
		{
			Console.Out.WriteLine(
				BenchmarkRunner.Run<BuildTests>());
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildSimpleProjectWithTabs()
		{
			return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\SimpleProject\tab\SimpleProject.sln");
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildSimpleProjectWithSpaces()
		{
			return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\SimpleProject\space\SimpleProject.sln");
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildRocksWithTabs()
		{
			return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\tab\Rocks.sln");
		}

		[Benchmark]
		public async Task<ImmutableList<EmitResult>> BuildRocksWithSpaces()
		{
			return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\space\Rocks.sln");
		}

		private static async Task<ImmutableList<EmitResult>> RunCompilation(string solutionFile)
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
				using (var stream = new MemoryStream())
				{
					var compilation = await solution.GetProject(projectId).GetCompilationAsync();
					compilation = compilation.WithOptions(CreateOptions(compilation.Options));
					var emitResult = compilation.Emit(stream);

					if (!emitResult.Success)
					{
						throw new BuildException(emitResult.Diagnostics);
					}
					else
					{
						results.Add(emitResult);
					}
				}
			}

			return results.ToImmutableList();
		}
	}
}
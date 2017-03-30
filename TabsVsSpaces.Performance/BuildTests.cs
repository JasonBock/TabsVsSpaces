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
public async Task<ImmutableList<EmitResult>> BuildAngleSharpWithTabs()
{
	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\tab\src\AngleSharp.Core.sln");
}
[Benchmark]
public async Task<ImmutableList<EmitResult>> BuildAngleSharpWithSpaces()
{
	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\space\src\AngleSharp.Core.sln");
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
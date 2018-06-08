using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TabsVsSpaces
{
	public sealed class PerformanceRunner
	{
		private const string BuildTestsFile = "BuildTests.cs";
		private const string IndentStyleTabs = "Tabs";
		private const string IndentStyleSpaces = "Spaces";
		private const string PerformanceSolutionFile = "TabsVsSpaces.Performance.sln";
		private const string PerformanceSolutionPath = @"..\..\..\TabsVsSpaces.Performance";

		private readonly Reformatter[] reformatters;

		public PerformanceRunner(Reformatter[] reformatters) => 
			this.reformatters = reformatters;

		public async Task RunAsync()
		{
			var testMethods = new List<string>();

			foreach (var reformatter in this.reformatters)
			{
				testMethods.Add(PerformanceRunner.GetTestMethod(reformatter.Configuration.SourceFolder,
					PerformanceRunner.IndentStyleTabs,
					Path.Combine(reformatter.Directories.tabDirectory, reformatter.Configuration.SolutionFile)));
				testMethods.Add(PerformanceRunner.GetTestMethod(reformatter.Configuration.SourceFolder,
					PerformanceRunner.IndentStyleSpaces,
					Path.Combine(reformatter.Directories.spaceDirectory, reformatter.Configuration.SolutionFile)));
			}

			var testClassCode = PerformanceRunner.GetTestClass(testMethods);
			var testClassFile = Path.Combine(PerformanceRunner.PerformanceSolutionPath, PerformanceRunner.BuildTestsFile);

			await Console.Out.WriteLineAsync($"Updating test class: {testClassFile}...");
			File.WriteAllText(testClassFile, testClassCode);

			var solutionPath = Path.Combine(PerformanceRunner.PerformanceSolutionPath, PerformanceRunner.PerformanceSolutionFile);
			await Console.Out.WriteLineAsync($"Updating solution: {solutionPath}...");

			var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(solutionPath);
			var project = solution.GetProject(solution.ProjectIds[0]);
			var compilation = await project.GetCompilationAsync();
			await Console.Out.WriteLineAsync($"Project optimization level: {compilation.Options.OptimizationLevel}...");

			var file = Path.Combine($"{Path.GetFileNameWithoutExtension(PerformanceRunner.PerformanceSolutionFile)}.dll");

			using (var stream = new MemoryStream())
			{
				var emitResult = compilation.Emit(stream);

				if (!emitResult.Success)
				{
					throw new BuildException(emitResult.Diagnostics);
				}
			}
		}

		private static string GetTestMethod(string solutionName, string indentStyle, string solutionFile) =>
$@"[Benchmark]
public async Task<ImmutableList<EmitResult>> Build{solutionName}With{indentStyle}()
{{
	return await BuildTests.RunCompilation(@""{solutionFile}"");
}}";

		private static string GetTestClass(IEnumerable<string> testMethods) =>
$@"using BenchmarkDotNet.Attributes;
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
{{
	public class BuildTests
	{{
		public static void Run()
		{{
			Console.Out.WriteLine(
				BenchmarkRunner.Run<BuildTests>());
		}}
		
		{string.Join(Environment.NewLine, testMethods)}
		
		private static async Task<ImmutableList<EmitResult>> RunCompilation(string solutionFile)
		{{
			CSharpCompilationOptions CreateOptions(CompilationOptions options)
			{{
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
			}}

			var results = new List<EmitResult>();
			var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(solutionFile);

			foreach (var projectId in solution.GetProjectDependencyGraph().GetTopologicallySortedProjects())
			{{
				using (var stream = new MemoryStream())
				{{
					var compilation = await solution.GetProject(projectId).GetCompilationAsync();
					compilation = compilation.WithOptions(CreateOptions(compilation.Options));
					var emitResult = compilation.Emit(stream);

					if (!emitResult.Success)
					{{
						throw new BuildException(emitResult.Diagnostics);
					}}
					else
					{{
						results.Add(emitResult);
					}}
				}}
			}}

			return results.ToImmutableList();
		}}
	}}
}}";
	}
}
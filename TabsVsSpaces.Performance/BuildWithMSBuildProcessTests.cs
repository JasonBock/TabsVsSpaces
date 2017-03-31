using BenchmarkDotNet.Attributes;
using System.Diagnostics;

namespace TabsVsSpaces.Performance
{
	public class BuildWithMSBuildProcessTests
	{
		[Benchmark]
		public int BuildCslaWithTabs()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Csla\tab\Source\csla.net.sln");
		}

		[Benchmark]
		public int BuildCslaWithSpaces()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Csla\space\Source\csla.net.sln");
		}

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

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAutofacWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\tab\Autofac.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAutofacWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\space\Autofac.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAutoMapperWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\tab\src\AutoMapper.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildAutoMapperWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\space\src\AutoMapper.sln");
		//}

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

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNodaTimeWithTabs()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\tab\src\NodaTime-All.sln");
		//}

		//[Benchmark]
		//public async Task<ImmutableList<EmitResult>> BuildNodaTimeWithSpaces()
		//{
		//	return await BuildTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\space\src\NodaTime-All.sln");
		//}

		private static int RunCompilation(string solutionFile)
		{
			var msBuildStartInfo = new ProcessStartInfo();
			msBuildStartInfo.CreateNoWindow = false;
			msBuildStartInfo.UseShellExecute = false;
			msBuildStartInfo.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe";
			msBuildStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			msBuildStartInfo.Arguments = $"\"{solutionFile}\" /t:Clean;Build /p:Configuration=Release /m /noconlog";
			msBuildStartInfo.WorkingDirectory = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin";

			using (var msBuildProcess = Process.Start(msBuildStartInfo))
			{
				msBuildProcess.WaitForExit();

				if(msBuildProcess.ExitCode != 0)
				{
					throw new MSBuildException(msBuildProcess.ExitCode);
				}

				return msBuildProcess.ExitCode;
			}
		}
	}
}
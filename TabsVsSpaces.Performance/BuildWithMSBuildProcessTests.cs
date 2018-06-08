using BenchmarkDotNet.Attributes;
using System.Diagnostics;

namespace TabsVsSpaces.Performance
{
	public class BuildWithMSBuildProcessTests
	{
		//[Benchmark]
		//public int BuildCslaWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Csla\tab\Source\csla.net.sln");
		//}

		//[Benchmark]
		//public int BuildCslaWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Csla\space\Source\csla.net.sln");
		//}

		//[Benchmark]
		//public int BuildRocksWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\tab\Rocks.sln");
		//}

		//[Benchmark]
		//public int BuildRocksWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Rocks\space\Rocks.sln");
		//}

		//[Benchmark]
		//public int BuildAutofacWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\tab\Autofac.sln");
		//}

		//[Benchmark]
		//public int BuildAutofacWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Autofac\space\Autofac.sln");
		//}

		//[Benchmark]
		//public int BuildAutoMapperWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\tab\src\AutoMapper.sln");
		//}

		//[Benchmark]
		//public int BuildAutoMapperWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\AutoMapper\space\src\AutoMapper.sln");
		//}

		//[Benchmark]
		//public int BuildMoqWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Moq\tab\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public int BuildMoqWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Moq\space\src\Moq.Sdk.sln");
		//}

		//[Benchmark]
		//public int BuildNewtonsoftJsonWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Newtonsoft.Json\tab\Src\Newtonsoft.Json.sln");
		//}

		//[Benchmark]
		//public int BuildNewtonsofJsonWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\Newtonsoft.Json\space\Src\Newtonsoft.Json.sln");
		//}

		[Benchmark]
		public int BuildAngleSharpWithTabs()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\tab\src\AngleSharp.Core.sln");
		}

		[Benchmark]
		public int BuildAngleSharpWithSpaces()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\AngleSharp\space\src\AngleSharp.Core.sln");
		}

		//[Benchmark]
		//public int BuildNLogWithTabs()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\NLog\tab\src\NLog.netfx45.sln");
		//}

		//[Benchmark]
		//public int BuildNLogWithSpaces()
		//{
		//	return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\NLog\space\src\NLog.netfx45.sln");
		//}

		[Benchmark]
		public int BuildNodaTimeWithTabs()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\tab\src\NodaTime-All.sln");
		}

		[Benchmark]
		public int BuildNodaTimeWithSpaces()
		{
			return BuildWithMSBuildProcessTests.RunCompilation(@"M:\TVSSource\Repos\NodaTime\space\src\NodaTime-All.sln");
		}

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
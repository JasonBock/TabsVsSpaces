using TabsVsSpaces.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TabsVsSpaces
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// Done:
			// * Rocks (https://github.com/jasonbock/rocks)
			// * Autofac (https://github.com/autofac/Autofac)
			// * AutoMapper (https://github.com/AutoMapper/AutoMapper)
			// * CSLA (https://github.com/marimerLLC/csla)
			// * Moq (https://github.com/moq/moq)
			// * Json.Net (https://github.com/JamesNK/Newtonsoft.Json)
			// * AngleSharp (https://github.com/AngleSharp/AngleSharp)
			// * NLog (https://github.com/NLog/NLog/)
			// * NodaTime (https://github.com/nodatime/nodatime)
			var configuration = new CodeConfiguration(
				new CodeItemConfiguration("NodaTime", @"src\NodaTime-All.sln"));

			var reformatters = new Reformatter[configuration.Items.Length];

			for (var i = 0; i < configuration.Items.Length; i++)
			{
				reformatters[i] = new Reformatter(configuration.Items[i]);
				await reformatters[i].ReformatAsync();
			}

			var performance = new PerformanceRunner(reformatters);
			await performance.RunAsync();

			foreach (var reformatter in reformatters)
			{
				var size = new SolutionComparisonSize(
					(Path.Combine(reformatter.Directories.tabDirectory, reformatter.Configuration.SolutionFile),
					Path.Combine(reformatter.Directories.spaceDirectory, reformatter.Configuration.SolutionFile)));
				Console.Out.WriteLine(
					$"Sizes for {reformatter.Configuration.SourceFolder}, tab is {size.TabSize}, space is {size.SpaceSize}, difference is {size.TabSmallerBy}");
			}

			//var size = new SolutionComparisonSize(
			//	(@"M:\TVSSource\Repos\Newtonsoft.Json\tab\Src\Newtonsoft.Json.sln",
			//	@"M:\TVSSource\Repos\Newtonsoft.Json\space\Src\Newtonsoft.Json.sln"));
			//Console.Out.WriteLine(
			//	$"Sizes for Newtonsoft.Json, tab is {size.TabSize}, space is {size.SpaceSize}, difference is {size.TabSmallerBy}");
		}
	}
}
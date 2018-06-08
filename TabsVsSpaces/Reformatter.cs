using EnvDTE;
using Microsoft.CodeAnalysis.MSBuild;
using TabsVsSpaces.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TabsVsSpaces
{
	public sealed class Reformatter
	{
		public Reformatter(CodeItemConfiguration configuration) => 
			this.Configuration = configuration;

		public async Task ReformatAsync()
		{
			var (tabDirectory, spaceDirectory) = this.SetupDirectories();
			this.SetupEditorConfig(tabDirectory, Constants.EditorConfig.IndentStyleTab);
			this.SetupEditorConfig(spaceDirectory, Constants.EditorConfig.IndentStyleSpace);

			await Reformatter.FormatFiles(tabDirectory, this.Configuration.SolutionFile);
			await Reformatter.FormatFiles(spaceDirectory, this.Configuration.SolutionFile);

			this.Directories = (tabDirectory, spaceDirectory);
		}

		private void SetupEditorConfig(string rootPath, string indentStyle)
		{
			void DeleteAllEditorConfigFiles(string directory)
			{
				var config = Path.Combine(directory, Constants.EditorConfig.FileName);

				if (File.Exists(config))
				{
					File.Delete(config);
				}

				foreach (var childDirectory in Directory.GetDirectories(directory))
				{
					DeleteAllEditorConfigFiles(childDirectory);
				}
			}

			Console.Out.WriteLine($"Removing all .editorconfig files from {rootPath}...");
			DeleteAllEditorConfigFiles(rootPath);

			Console.Out.WriteLine("Creating .editorconfig...");
			var rootConfig = Path.Combine(rootPath, Constants.EditorConfig.FileName);

			File.WriteAllLines(rootConfig,
				new[]
				{
					"root = true",
					string.Empty,
					"[*.cs]",
					$"indent_style = {indentStyle}",
					"indent_size = 3"
				});
		}

		private static void CopyDirectory(string source, string destination)
		{
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}

			var startInfo = new ProcessStartInfo
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				FileName = "robocopy",
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = $"\"{source}\" \"{destination}\" /E"
			};

			using (var process = System.Diagnostics.Process.Start(startInfo))
			{
				process.WaitForExit();
			}
		}

		private static async Task FormatFiles(string directory, string solutionFile)
		{
			await Console.Out.WriteLineAsync($"Editing solution content for {directory}...");
			var vsType = Type.GetTypeFromProgID("VisualStudio.DTE.15.0");
			var vs = Activator.CreateInstance(vsType) as DTE;

			try
			{
				var solutionOpenedEvent = new ManualResetEvent(false);
				vs.Events.SolutionEvents.Opened += () => solutionOpenedEvent.Set();
				vs.MainWindow.Visible = true;

				var solutionPath = Path.Combine(directory, solutionFile);
				vs.ExecuteCommand("File.OpenProject", $"\"{solutionPath}\"");
				solutionOpenedEvent.WaitOne();

				// HACK: I can't figure out a solid way to really know when VS is "done", 
				// so I'll wait for Constants.WaitTimes.SolutionOpened :(

				await Console.Out.WriteLineAsync($"Solution loaded, waiting {Constants.WaitTimes.SolutionOpened} ...");
				await Task.Delay(Constants.WaitTimes.SolutionOpened);

				var workspace = MSBuildWorkspace.Create();
				var solution = await workspace.OpenSolutionAsync(solutionPath);

				foreach (var projectId in solution.ProjectIds)
				{
					var project = solution.GetProject(projectId);

					foreach (var documentId in project.DocumentIds)
					{
						var document = solution.GetDocument(documentId);

						if (Path.GetExtension(document.FilePath).ToLower() == ".cs")
						{
							await Console.Out.WriteLineAsync($"Opening {document.FilePath}...");
							vs.ExecuteCommand("File.OpenFile", "\"" + document.FilePath + "\"");
							await Console.Out.WriteLineAsync($"Waiting {Constants.WaitTimes.CommandExecuted} ...");
							await Task.Delay(Constants.WaitTimes.CommandExecuted);

							await Console.Out.WriteLineAsync($"Formatting {document.FilePath}...");
							vs.ExecuteCommand("Edit.FormatDocument");
							await Console.Out.WriteLineAsync($"Waiting {Constants.WaitTimes.CommandExecuted} ...");
							await Task.Delay(Constants.WaitTimes.CommandExecuted);

							await Console.Out.WriteLineAsync($"Saving {document.FilePath}...");
							vs.ExecuteCommand("File.SaveAll");
							await Console.Out.WriteLineAsync($"Waiting {Constants.WaitTimes.CommandExecuted} ...");
							await Task.Delay(Constants.WaitTimes.CommandExecuted);

							await Console.Out.WriteLineAsync($"Closing {document.FilePath}...");
							vs.ExecuteCommand("File.Close");
							await Console.Out.WriteLineAsync($"Waiting {Constants.WaitTimes.CommandExecuted} ...");
							await Task.Delay(Constants.WaitTimes.CommandExecuted);
						}
					}
				}
			}
			finally
			{
				vs.Application.Quit();
			}
		}

		private (string, string) SetupDirectories()
		{
			var sourceSolutionDirectory = Path.Combine(Constants.Folders.Source, this.Configuration.SourceFolder);
			var repoSolutionDirectory = Path.Combine(Constants.Folders.Repo, this.Configuration.SourceFolder);

			var tabDirectory = Path.Combine(repoSolutionDirectory, Constants.Folders.Tab);
			var spaceDirectory = Path.Combine(repoSolutionDirectory, Constants.Folders.Space);

			Reformatter.CopyDirectory(sourceSolutionDirectory, tabDirectory);
			Reformatter.CopyDirectory(sourceSolutionDirectory, spaceDirectory);

			return (tabDirectory, spaceDirectory);
		}

		public CodeItemConfiguration Configuration { get; }
		public (string tabDirectory, string spaceDirectory) Directories { get; private set; }
	}
}
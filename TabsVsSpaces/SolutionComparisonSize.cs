using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Linq;

namespace TabsVsSpaces
{
	public sealed class SolutionComparisonSize
	{
		public SolutionComparisonSize((string tabSolution, string spaceSolution) solutions)
		{
			// TODO: Should redo for the solution items ONLY as those CS files are the ones that got changed;
			// my current approach takes everything into consideration.
			this.TabSize = SolutionComparisonSize.GetSolutionSize(solutions.tabSolution);
			this.SpaceSize = SolutionComparisonSize.GetSolutionSize(solutions.spaceSolution);

			this.TabSmallerBy = ((double)Math.Abs(this.TabSize - this.SpaceSize) / (double)this.SpaceSize) * 100d;
		}

		private static long GetSolutionSize(string solutionPath)
		{
			var size = 0L;
			var workspace = MSBuildWorkspace.Create();
			var solutionTask = workspace.OpenSolutionAsync(solutionPath);
			solutionTask.Wait();
			var solution = solutionTask.Result;

			foreach (var projectId in solution.ProjectIds)
			{
				var project = solution.GetProject(projectId);

				foreach (var documentId in project.DocumentIds)
				{
					var document = solution.GetDocument(documentId);

					if (Path.GetExtension(document.FilePath).ToLower() == ".cs")
					{
						size += new FileInfo(document.FilePath).Length;
					}
				}
			}

			return size;
		}

		public double TabSmallerBy { get; }
		public long SpaceSize { get; }
		public long TabSize { get; }
	}
}

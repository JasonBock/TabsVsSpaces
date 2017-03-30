namespace TabsVsSpaces.Configuration
{
	public sealed class CodeItemConfiguration
	{
		public CodeItemConfiguration(string sourceFolder, string solutionFile)
		{
			this.SolutionFile = solutionFile;
			this.SourceFolder = sourceFolder;
		}

		public string SourceFolder { get; }
		public string SolutionFile { get; }
	}
}

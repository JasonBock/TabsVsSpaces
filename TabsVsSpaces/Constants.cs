using System;

namespace TabsVsSpaces
{
	public static class Constants
	{
		public static class EditorConfig
		{
			public const string FileName = ".editorconfig";
			public const string IndentStyleSpace = "space";
			public const string IndentStyleTab = "tab";
		}

		public static class Folders
		{
			public const string Repo = @"M:\TVSSource\Repos";
			public const string Source = @"M:\TVSSource\Source";
			public const string Space = "space";
			public const string Tab = "tab";
		}

		public static class WaitTimes
		{
			public static TimeSpan CommandExecuted = TimeSpan.FromSeconds(2);
			public static TimeSpan SolutionOpened = TimeSpan.FromSeconds(10);
		}
	}
}
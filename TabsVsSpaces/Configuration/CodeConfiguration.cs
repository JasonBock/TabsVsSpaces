using System.Collections.Immutable;

namespace TabsVsSpaces.Configuration
{
	public sealed class CodeConfiguration
	{
		public CodeConfiguration(params CodeItemConfiguration[] items)
		{
			this.Items = ImmutableArray.Create<CodeItemConfiguration>(items);
		}

		public ImmutableArray<CodeItemConfiguration> Items { get; }
	}
}

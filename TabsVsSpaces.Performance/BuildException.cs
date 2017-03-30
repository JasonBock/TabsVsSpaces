using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace TabsVsSpaces.Performance
{
	[Serializable]
	public sealed class BuildException
		: Exception
	{
		public BuildException()
		{
		}

		public BuildException(ImmutableArray<Diagnostic> diagnostics)
		{
			this.Diagnostics = diagnostics;
		}

		public BuildException(string message) : base(message)
		{
		}

		public BuildException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public BuildException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; }
	}
}

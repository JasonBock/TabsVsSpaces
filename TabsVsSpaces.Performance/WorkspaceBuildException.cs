using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace TabsVsSpaces.Performance
{
	[Serializable]
	public sealed class WorkspaceBuildException
		: Exception
	{
		public WorkspaceBuildException()
		{
		}

		public WorkspaceBuildException(ImmutableArray<Diagnostic> diagnostics)
		{
			this.Diagnostics = diagnostics;
		}

		public WorkspaceBuildException(string message) : base(message)
		{
		}

		public WorkspaceBuildException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public WorkspaceBuildException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; }
	}
}

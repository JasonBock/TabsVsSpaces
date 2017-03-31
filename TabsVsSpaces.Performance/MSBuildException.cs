using System;
using System.Runtime.Serialization;

namespace TabsVsSpaces.Performance
{
	[Serializable]
	internal class MSBuildException : Exception
	{
		private int exitCode;

		public MSBuildException()
		{
		}

		public MSBuildException(int exitCode)
		{
			this.exitCode = exitCode;
		}

		public MSBuildException(string message) : base(message)
		{
		}

		public MSBuildException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MSBuildException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
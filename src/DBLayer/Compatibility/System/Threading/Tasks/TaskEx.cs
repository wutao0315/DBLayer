using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	static class TaskEx
	{
		public static Task CompletedTask
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Task.CompletedTask;
			}
		}
	}
}

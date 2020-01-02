using System;

namespace Steamworks
{
	public static class CallbackDispatcher
	{
		public static void ExceptionHandler(global::System.Exception e)
		{
			global::System.Console.WriteLine(e.Message);
		}
	}
}

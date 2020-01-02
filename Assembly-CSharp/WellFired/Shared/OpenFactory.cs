using System;

namespace WellFired.Shared
{
	public static class OpenFactory
	{
		public static bool PlatformCanOpen()
		{
			return true;
		}

		public static global::WellFired.Shared.IOpen CreateOpen()
		{
			return new global::WellFired.Shared.WinOpen();
		}
	}
}

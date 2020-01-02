using System;

public static class KGFCoreVersion
{
	public static global::System.Version GetCurrentVersion()
	{
		return global::KGFCoreVersion.itsVersion.Clone() as global::System.Version;
	}

	private static global::System.Version itsVersion = new global::System.Version(1, 2, 0, 0);
}

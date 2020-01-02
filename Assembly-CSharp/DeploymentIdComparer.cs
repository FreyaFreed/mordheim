using System;
using System.Collections.Generic;

public class DeploymentIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DeploymentId>
{
	public bool Equals(global::DeploymentId x, global::DeploymentId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DeploymentId obj)
	{
		return (int)obj;
	}

	public static readonly global::DeploymentIdComparer Instance = new global::DeploymentIdComparer();
}

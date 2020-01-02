using System;
using System.Collections.Generic;

public class DeploymentScenarioMapLayoutIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DeploymentScenarioMapLayoutId>
{
	public bool Equals(global::DeploymentScenarioMapLayoutId x, global::DeploymentScenarioMapLayoutId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DeploymentScenarioMapLayoutId obj)
	{
		return (int)obj;
	}

	public static readonly global::DeploymentScenarioMapLayoutIdComparer Instance = new global::DeploymentScenarioMapLayoutIdComparer();
}

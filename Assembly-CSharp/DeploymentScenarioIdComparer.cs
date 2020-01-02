using System;
using System.Collections.Generic;

public class DeploymentScenarioIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DeploymentScenarioId>
{
	public bool Equals(global::DeploymentScenarioId x, global::DeploymentScenarioId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DeploymentScenarioId obj)
	{
		return (int)obj;
	}

	public static readonly global::DeploymentScenarioIdComparer Instance = new global::DeploymentScenarioIdComparer();
}

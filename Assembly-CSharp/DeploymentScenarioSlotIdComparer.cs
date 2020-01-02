using System;
using System.Collections.Generic;

public class DeploymentScenarioSlotIdComparer : global::System.Collections.Generic.IEqualityComparer<global::DeploymentScenarioSlotId>
{
	public bool Equals(global::DeploymentScenarioSlotId x, global::DeploymentScenarioSlotId y)
	{
		return x == y;
	}

	public int GetHashCode(global::DeploymentScenarioSlotId obj)
	{
		return (int)obj;
	}

	public static readonly global::DeploymentScenarioSlotIdComparer Instance = new global::DeploymentScenarioSlotIdComparer();
}

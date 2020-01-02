using System;
using System.Collections.Generic;

[global::System.Serializable]
public class WarbandInitData
{
	public string name;

	public global::WarbandId id;

	public int team;

	public int rank;

	public global::PlayerTypeId playerId;

	public global::DeploymentScenarioSlotId deployId;

	public global::PrimaryObjectiveTypeId objectiveTypeId;

	public int objectiveTargetIdx;

	public global::System.Collections.Generic.List<global::UnitInitData> units;
}

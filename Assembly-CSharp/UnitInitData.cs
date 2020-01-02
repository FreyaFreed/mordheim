using System;
using System.Collections.Generic;

[global::System.Serializable]
public class UnitInitData
{
	public string name;

	public global::UnitId id;

	public global::UnitRankId rank;

	public global::System.Collections.Generic.List<global::WeaponInitData> weapons;

	public global::System.Collections.Generic.List<global::SkillId> skills;

	public global::System.Collections.Generic.List<global::SkillId> spells;

	public global::System.Collections.Generic.List<global::MutationId> mutations;

	public global::System.Collections.Generic.List<global::InjuryId> injuries;
}

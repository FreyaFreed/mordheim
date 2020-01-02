using System;
using Mono.Data.Sqlite;

public class SkillFxData : global::DataCore
{
	public int Id { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::SequenceId SequenceId { get; private set; }

	public string RightHandFx { get; private set; }

	public string LeftHandFx { get; private set; }

	public string ChargeFx { get; private set; }

	public bool ChargeOnTarget { get; private set; }

	public string LaunchFx { get; private set; }

	public string FizzleFx { get; private set; }

	public string ProjectileFx { get; private set; }

	public string ImpactFx { get; private set; }

	public string HitFx { get; private set; }

	public string TrailColor { get; private set; }

	public bool ProjFromTarget { get; private set; }

	public bool OverrideVariation { get; private set; }

	public int Variation { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
		this.SequenceId = (global::SequenceId)reader.GetInt32(2);
		this.RightHandFx = reader.GetString(3);
		this.LeftHandFx = reader.GetString(4);
		this.ChargeFx = reader.GetString(5);
		this.ChargeOnTarget = (reader.GetInt32(6) != 0);
		this.LaunchFx = reader.GetString(7);
		this.FizzleFx = reader.GetString(8);
		this.ProjectileFx = reader.GetString(9);
		this.ImpactFx = reader.GetString(10);
		this.HitFx = reader.GetString(11);
		this.TrailColor = reader.GetString(12);
		this.ProjFromTarget = (reader.GetInt32(13) != 0);
		this.OverrideVariation = (reader.GetInt32(14) != 0);
		this.Variation = reader.GetInt32(15);
	}
}

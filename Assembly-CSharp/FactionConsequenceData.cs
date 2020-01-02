using System;
using Mono.Data.Sqlite;

public class FactionConsequenceData : global::DataCore
{
	public int Id { get; private set; }

	public global::FactionId FactionId { get; private set; }

	public int LateShipmentCount { get; private set; }

	public int NextShipmentRequestModifierPerc { get; private set; }

	public int NextShipmentGoldRewardModifierPerc { get; private set; }

	public global::FactionConsequenceTargetId FactionConsequenceTargetId { get; private set; }

	public global::InjuryId InjuryId { get; private set; }

	public int TreatmentTime { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.FactionId = (global::FactionId)reader.GetInt32(1);
		this.LateShipmentCount = reader.GetInt32(2);
		this.NextShipmentRequestModifierPerc = reader.GetInt32(3);
		this.NextShipmentGoldRewardModifierPerc = reader.GetInt32(4);
		this.FactionConsequenceTargetId = (global::FactionConsequenceTargetId)reader.GetInt32(5);
		this.InjuryId = (global::InjuryId)reader.GetInt32(6);
		this.TreatmentTime = reader.GetInt32(7);
	}
}

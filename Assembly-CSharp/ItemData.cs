using System;
using Mono.Data.Sqlite;

public class ItemData : global::DataCore
{
	public global::ItemId Id { get; private set; }

	public string Name { get; private set; }

	public string Asset { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public bool Backup { get; private set; }

	public bool Paired { get; private set; }

	public bool Climbing { get; private set; }

	public bool Lootable { get; private set; }

	public bool Sellable { get; private set; }

	public bool LockSlot { get; private set; }

	public bool Stackable { get; private set; }

	public bool Undroppable { get; private set; }

	public bool MutationBased { get; private set; }

	public int PriceBuy { get; private set; }

	public int PriceSold { get; private set; }

	public int Rating { get; private set; }

	public int DamageMin { get; private set; }

	public int DamageMax { get; private set; }

	public int ArmorAbsorption { get; private set; }

	public global::ItemSpeedId ItemSpeedId { get; private set; }

	public global::ItemRangeId ItemRangeId { get; private set; }

	public global::TargetingId TargetingId { get; private set; }

	public global::AnimStyleId AnimStyleId { get; private set; }

	public global::BoneId BoneId { get; private set; }

	public global::ProjectileId ProjectileId { get; private set; }

	public int Shots { get; private set; }

	public int Radius { get; private set; }

	public string Sound { get; private set; }

	public string SoundCat { get; private set; }

	public bool TargetAlly { get; private set; }

	public bool IsIdol { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Asset = reader.GetString(2);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(3);
		this.Backup = (reader.GetInt32(4) != 0);
		this.Paired = (reader.GetInt32(5) != 0);
		this.Climbing = (reader.GetInt32(6) != 0);
		this.Lootable = (reader.GetInt32(7) != 0);
		this.Sellable = (reader.GetInt32(8) != 0);
		this.LockSlot = (reader.GetInt32(9) != 0);
		this.Stackable = (reader.GetInt32(10) != 0);
		this.Undroppable = (reader.GetInt32(11) != 0);
		this.MutationBased = (reader.GetInt32(12) != 0);
		this.PriceBuy = reader.GetInt32(13);
		this.PriceSold = reader.GetInt32(14);
		this.Rating = reader.GetInt32(15);
		this.DamageMin = reader.GetInt32(16);
		this.DamageMax = reader.GetInt32(17);
		this.ArmorAbsorption = reader.GetInt32(18);
		this.ItemSpeedId = (global::ItemSpeedId)reader.GetInt32(19);
		this.ItemRangeId = (global::ItemRangeId)reader.GetInt32(20);
		this.TargetingId = (global::TargetingId)reader.GetInt32(21);
		this.AnimStyleId = (global::AnimStyleId)reader.GetInt32(22);
		this.BoneId = (global::BoneId)reader.GetInt32(23);
		this.ProjectileId = (global::ProjectileId)reader.GetInt32(24);
		this.Shots = reader.GetInt32(25);
		this.Radius = reader.GetInt32(26);
		this.Sound = reader.GetString(27);
		this.SoundCat = reader.GetString(28);
		this.TargetAlly = (reader.GetInt32(29) != 0);
		this.IsIdol = (reader.GetInt32(30) != 0);
	}
}

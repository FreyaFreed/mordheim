using System;

[global::System.Serializable]
public class SearchSlotData
{
	public global::ItemId itemId;

	public global::ItemQualityId itemQualityId = global::ItemQualityId.NORMAL;

	public global::RuneMarkId runeMarkId;

	public global::AllegianceId allegianceId;

	public global::RuneMarkQualityId runeMarkQualityId = global::RuneMarkQualityId.REGULAR;

	public global::ItemId restrictedItemId;

	public global::ItemTypeId restrictedItemTypeId;

	public int value;
}

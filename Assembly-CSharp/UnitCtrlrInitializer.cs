using System;
using UnityEngine;

public class UnitCtrlrInitializer : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnitSave unitSave = new global::UnitSave(this.id);
		unitSave.items[2] = new global::ItemSave(this.itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		if (this.offItemId != global::ItemId.NONE)
		{
			unitSave.items[3] = new global::ItemSave(this.offItemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		}
		unitSave.items[4] = new global::ItemSave(this.secItemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		if (this.secOffItemId != global::ItemId.NONE)
		{
			unitSave.items[5] = new global::ItemSave(this.secOffItemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		}
		unitSave.items[1] = new global::ItemSave(this.armordId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		unitSave.spells.Add(this.areaSpellId);
		unitSave.spells.Add(this.pointSpellId);
		unitSave.activeSkills.Add(this.activeSkillId);
		global::UnitMenuController component = base.GetComponent<global::UnitMenuController>();
		if (component != null)
		{
			component.SetCharacter(new global::Unit(unitSave));
			component.LoadBodyParts();
		}
		global::UnitController component2 = base.GetComponent<global::UnitController>();
		if (component2 != null)
		{
			component2.FirstSyncInit(unitSave, 0U, 0, 0, global::PlayerTypeId.PLAYER, 0, true, true);
			base.GetComponent<global::UnityEngine.Rigidbody>().interpolation = global::UnityEngine.RigidbodyInterpolation.Interpolate;
		}
		global::UnityEngine.Animator component3 = base.GetComponent<global::UnityEngine.Animator>();
		component3.stabilizeFeet = true;
	}

	public global::UnitId id;

	public global::ItemId itemId;

	public global::ItemId offItemId;

	public global::ItemId secItemId;

	public global::ItemId secOffItemId;

	public global::ItemId armordId;

	public global::SkillId areaSpellId;

	public global::SkillId pointSpellId;

	public global::SkillId activeSkillId;
}

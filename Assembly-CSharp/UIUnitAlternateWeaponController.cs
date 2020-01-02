using System;

public class UIUnitAlternateWeaponController : global::UIUnitControllerChanged
{
	protected virtual void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_WEAPON_CHANGED, new global::DelReceiveNotice(this.OnWeaponSwapped));
	}

	private void OnWeaponSwapped()
	{
		global::UnitController y = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
		if (base.CurrentUnitController != null && base.CurrentUnitController == y)
		{
			this.UnitWeaponSwapped(base.CurrentUnitController);
		}
	}

	public void UnitWeaponSwapped(global::UnitController unitController)
	{
		global::Item item = unitController.unit.Items[(int)unitController.unit.InactiveWeaponSlot];
		global::Item item2 = unitController.unit.Items[(int)(unitController.unit.InactiveWeaponSlot + 1)];
		if (unitController.CanSwitchWeapon())
		{
			this.mainHand.gameObject.SetActive(true);
			this.offHand.gameObject.SetActive(true);
			this.mainHand.Set(item);
			this.offHand.Set(item2);
		}
		else
		{
			this.mainHand.gameObject.SetActive(false);
			this.offHand.gameObject.SetActive(false);
		}
	}

	protected override void OnUnitChanged()
	{
		if (base.CurrentUnitController != null)
		{
			this.UnitWeaponSwapped(base.CurrentUnitController);
		}
		else
		{
			this.mainHand.gameObject.SetActive(false);
			this.offHand.gameObject.SetActive(false);
		}
	}

	public global::UIUnitAlternateWeaponGroup mainHand;

	public global::UIUnitAlternateWeaponGroup offHand;
}

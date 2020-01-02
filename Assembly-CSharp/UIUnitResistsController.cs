using System;
using UnityEngine.UI;

public class UIUnitResistsController : global::UIUnitControllerChanged
{
	protected virtual void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_ATTRIBUTES_CHANGED, new global::DelReceiveNotice(this.OnAttributesChanged));
	}

	private void OnAttributesChanged()
	{
		global::Unit unit = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::Unit;
		if (base.CurrentUnitController != null && base.CurrentUnitController.unit == unit)
		{
			base.UpdateUnit = true;
		}
		else
		{
			global::UnitController y = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
			if (base.CurrentUnitController != null && base.CurrentUnitController == y)
			{
				base.UpdateUnit = true;
			}
		}
	}

	public void AttributesChanged()
	{
		if (base.CurrentUnitController != null)
		{
			global::Unit unit = base.CurrentUnitController.unit;
			for (int i = 0; i < this.resists.Length; i++)
			{
				this.resists[i].Set(base.CurrentUnitController.unit);
			}
			this.unitClass.text = unit.LocalizedType;
		}
	}

	protected override void OnUnitChanged()
	{
		this.AttributesChanged();
	}

	public global::UnitStatsGroup[] resists;

	public global::UnityEngine.UI.Text unitClass;
}

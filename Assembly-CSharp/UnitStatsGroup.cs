using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsGroup : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_ATTRIBUTES_CHANGED, new global::DelReceiveNotice(this.OnAttributesChanged));
	}

	private void OnAttributesChanged()
	{
		global::UnitController unitController = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
		if (unitController != null && unitController.unit == this.currentUnit)
		{
			this.Set(this.currentUnit);
		}
	}

	public void Set(global::Unit unit)
	{
		this.currentUnit = unit;
		int attribute = unit.GetAttribute(this.attributeId);
		int num = 0;
		bool enabled = false;
		bool enabled2 = false;
		global::System.Collections.Generic.List<global::AttributeMod> orNull = unit.attributeModifiers.GetOrNull(this.attributeId);
		if (orNull != null)
		{
			for (int i = 0; i < orNull.Count; i++)
			{
				if (orNull[i].type == global::AttributeMod.Type.BUFF)
				{
					enabled = true;
					num += orNull[i].modifier;
				}
				else if (orNull[i].type == global::AttributeMod.Type.DEBUFF)
				{
					enabled2 = true;
					num += orNull[i].modifier;
				}
			}
		}
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		if (num == 0)
		{
			stringBuilder.Append(attribute.ToConstantString());
			if (this.percent)
			{
				stringBuilder.Append('%');
			}
		}
		else
		{
			if (num > 0)
			{
				stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_green"));
			}
			else
			{
				stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_red"));
			}
			stringBuilder.Append(attribute.ToConstantString());
			if (this.percent)
			{
				stringBuilder.Append('%');
			}
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_end"));
			stringBuilder.Append('(');
			if (num > 0)
			{
				stringBuilder.Append('+');
			}
			stringBuilder.Append(num.ToConstantString());
			stringBuilder.Append(')');
		}
		this.text.text = stringBuilder.ToString();
		this.buff.enabled = enabled;
		this.debuff.enabled = enabled2;
	}

	private void LateUpdate()
	{
	}

	public global::AttributeId attributeId;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text text;

	public global::UnityEngine.UI.Image buff;

	public global::UnityEngine.UI.Image debuff;

	public bool percent;

	private global::Unit currentUnit;
}

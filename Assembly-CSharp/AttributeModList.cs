using System;
using System.Collections.Generic;

public class AttributeModList
{
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	public global::AttributeMod this[int index]
	{
		get
		{
			return this.list[index];
		}
	}

	public void Add(global::AttributeMod attributreMod, string effect = null, bool isPercent = false, bool isEnemyMod = false, bool negate = false)
	{
		if (negate)
		{
			attributreMod.Negate();
		}
		if (isPercent)
		{
			attributreMod.SetIsPercent(true);
		}
		if (isEnemyMod)
		{
			attributreMod.SetEnemyMod(true);
		}
		if (!string.IsNullOrEmpty(effect))
		{
			attributreMod.SetEffect(effect);
		}
		for (int i = 0; i < this.list.Count; i++)
		{
			if (this.list[i].IsSame(attributreMod))
			{
				global::AttributeMod value = this.list[i];
				value.modifier += attributreMod.modifier;
				value.count++;
				this.list[i] = value;
				return;
			}
		}
		this.list.Add(attributreMod);
	}

	public void AddRange(global::System.Collections.Generic.List<global::AttributeMod> attributreMods, string effect = null, bool isPercent = false, bool isEnemyMod = false, bool negate = false)
	{
		if (attributreMods != null)
		{
			for (int i = 0; i < attributreMods.Count; i++)
			{
				this.Add(attributreMods[i], effect, isPercent, isEnemyMod, negate);
			}
		}
	}

	public void AddRange(global::AttributeModList attributreMods)
	{
		this.list.AddRange(attributreMods.list);
	}

	public void Clear()
	{
		this.list.Clear();
	}

	private readonly global::System.Collections.Generic.List<global::AttributeMod> list = new global::System.Collections.Generic.List<global::AttributeMod>();
}

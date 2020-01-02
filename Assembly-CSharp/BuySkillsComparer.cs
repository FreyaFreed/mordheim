using System;
using System.Collections.Generic;

public class BuySkillsComparer : global::System.Collections.Generic.IComparer<global::SkillData>
{
	public int Compare(global::SkillData x, global::SkillData y)
	{
		int num = x.StatValue.CompareTo(y.StatValue);
		if (num == 0)
		{
			return string.Compare(x.Name, y.Name, global::System.StringComparison.Ordinal);
		}
		return num;
	}
}

using System;
using System.Collections.Generic;

public class CompareItem : global::System.Collections.Generic.IComparer<global::Item>
{
	int global::System.Collections.Generic.IComparer<global::Item>.Compare(global::Item x, global::Item y)
	{
		if (x.QualityData.Id > y.QualityData.Id)
		{
			return -1;
		}
		if (x.QualityData.Id < y.QualityData.Id)
		{
			return 1;
		}
		if (x.Save.runeMarkQualityId > y.Save.runeMarkQualityId)
		{
			return -1;
		}
		if (x.Save.runeMarkQualityId < y.Save.runeMarkQualityId)
		{
			return 1;
		}
		if (x.PriceBuy > y.PriceBuy)
		{
			return -1;
		}
		if (x.PriceBuy < y.PriceBuy)
		{
			return 1;
		}
		return string.Compare(x.Name, y.Name);
	}
}

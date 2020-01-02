using System;
using System.Collections.Generic;

public class ProcWarbandRankJoinUnitTypeDataSorter : global::System.Collections.Generic.IComparer<global::ProcWarbandRankJoinUnitTypeData>
{
	int global::System.Collections.Generic.IComparer<global::ProcWarbandRankJoinUnitTypeData>.Compare(global::ProcWarbandRankJoinUnitTypeData x, global::ProcWarbandRankJoinUnitTypeData y)
	{
		int rating = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeData>((int)x.UnitTypeId).Rating;
		int rating2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeData>((int)y.UnitTypeId).Rating;
		if (rating == rating2)
		{
			return 0;
		}
		if (rating < rating2)
		{
			return 1;
		}
		if (rating > rating2)
		{
			return -1;
		}
		return 0;
	}
}

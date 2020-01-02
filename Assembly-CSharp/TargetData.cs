using System;
using System.Collections.Generic;

public class TargetData
{
	public TargetData(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
		this.boneTargetRange = new global::System.Collections.Generic.List<global::BoneTargetRange>();
		this.boneTargetRangeBlockingUnit = new global::System.Collections.Generic.List<global::BoneTargetRange>();
		for (int i = 0; i < ctrlr.boneTargets.Count; i++)
		{
			this.boneTargetRange.Add(new global::BoneTargetRange());
			this.boneTargetRangeBlockingUnit.Add(new global::BoneTargetRange());
		}
	}

	public float GetCover(bool blockingUnit)
	{
		int num = 0;
		global::System.Collections.Generic.List<global::BoneTargetRange> list = (!blockingUnit) ? this.boneTargetRange : this.boneTargetRangeBlockingUnit;
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].hitBone)
			{
				num++;
			}
		}
		return (float)num / (float)list.Count;
	}

	public global::UnitController unitCtrlr;

	public global::System.Collections.Generic.List<global::BoneTargetRange> boneTargetRange;

	public global::System.Collections.Generic.List<global::BoneTargetRange> boneTargetRangeBlockingUnit;
}

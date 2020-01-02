using System;
using System.Collections.Generic;
using UnityEngine;

public class PropRestrictions
{
	public PropRestrictions(global::PropRestrictionJoinPropTypeData data)
	{
		this.restrictionData = data;
		this.props = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
	}

	public global::PropRestrictionJoinPropTypeData restrictionData;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> props;
}

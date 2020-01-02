using System;
using UnityEngine;

public class UIWheelMutationSlot : global::UIWheelSlot
{
	public global::BodyPartId partId;

	public bool hiddingSlot;

	[global::UnityEngine.HideInInspector]
	public int unitMutationIdx;
}

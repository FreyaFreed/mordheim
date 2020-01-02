using System;
using UnityEngine;

public class AoeUnitChecker
{
	public AoeUnitChecker(global::UnitController ctrlr, global::UnityEngine.Vector3 lastPos)
	{
		this.ctrlr = ctrlr;
		this.lastPos = lastPos;
	}

	public global::UnitController ctrlr;

	public global::UnityEngine.Vector3 lastPos;
}

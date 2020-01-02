using System;
using UnityEngine;

public class LayerMaskManager
{
	public static global::UnityEngine.LayerMask rangeTargetMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("characters") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("target") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground");

	public static global::UnityEngine.LayerMask rangeTargetMaskNoChar = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("target") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground");

	public static global::UnityEngine.LayerMask fowMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground");

	public static global::UnityEngine.LayerMask footMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground");

	public static global::UnityEngine.LayerMask chargeMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("characters") | 1 << global::UnityEngine.LayerMask.NameToLayer("engage_circles") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("collision_wall");

	public static global::UnityEngine.LayerMask pathMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("characters") | 1 << global::UnityEngine.LayerMask.NameToLayer("engage_circles") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("collision_wall");

	public static global::UnityEngine.LayerMask decisionMask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("collision_wall");

	public static global::UnityEngine.LayerMask groundMask = 1 << global::UnityEngine.LayerMask.NameToLayer("ground") | 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small");

	public static global::UnityEngine.LayerMask groundOnlyMask = 1 << global::UnityEngine.LayerMask.NameToLayer("ground");

	public static global::UnityEngine.LayerMask overviewMask = 1 << global::UnityEngine.LayerMask.NameToLayer("characters") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small") | 1 << global::UnityEngine.LayerMask.NameToLayer("Ignore Raycast") | 1 << global::UnityEngine.LayerMask.NameToLayer("mapsystem");

	public static global::UnityEngine.LayerMask menuNodeMask = 1 << global::UnityEngine.LayerMask.NameToLayer("characters") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_small");

	public static int charactersLayer = global::UnityEngine.LayerMask.NameToLayer("characters");

	public static int engage_circlesLayer = global::UnityEngine.LayerMask.NameToLayer("engage_circles");
}

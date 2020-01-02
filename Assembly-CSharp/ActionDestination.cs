using System;
using Pathfinding;
using UnityEngine;

[global::System.Serializable]
public class ActionDestination
{
	public global::UnitActionId actionId = global::UnitActionId.CLIMB_3M;

	public global::ActionZone destination;

	public global::UnityEngine.GameObject fx;

	public global::Pathfinding.NodeLink2 navLink;

	public global::UnityEngine.ParticleSystem[] particles;
}

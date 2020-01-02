using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	public interface IAgent
	{
		global::UnityEngine.Vector2 Position { get; set; }

		float ElevationCoordinate { get; set; }

		global::UnityEngine.Vector2 CalculatedTargetPoint { get; }

		float CalculatedSpeed { get; }

		void SetTarget(global::UnityEngine.Vector2 targetPoint, float desiredSpeed, float maxSpeed);

		bool Locked { get; set; }

		float Radius { get; set; }

		float Height { get; set; }

		float NeighbourDist { get; set; }

		float AgentTimeHorizon { get; set; }

		float ObstacleTimeHorizon { get; set; }

		int MaxNeighbours { get; set; }

		int NeighbourCount { get; }

		global::Pathfinding.RVO.RVOLayer Layer { get; set; }

		global::Pathfinding.RVO.RVOLayer CollidesWith { get; set; }

		bool DebugDraw { get; set; }

		global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> NeighbourObstacles { get; }

		global::Pathfinding.RVO.MovementMode MovementMode { get; set; }

		float Priority { get; set; }

		global::System.Action PreCalculationCallback { set; }

		void SetCollisionNormal(global::UnityEngine.Vector2 normal);

		void ForceSetVelocity(global::UnityEngine.Vector2 velocity);
	}
}

using System;
using UnityEngine;

namespace Flowmap
{
	[global::System.Serializable]
	internal struct SimulationData
	{
		public float height;

		public float fluid;

		public float addFluid;

		public float removeFluid;

		public global::UnityEngine.Vector3 force;

		public global::UnityEngine.Vector4 outflow;

		public global::UnityEngine.Vector2 velocity;

		public global::UnityEngine.Vector3 velocityAccumulated;
	}
}

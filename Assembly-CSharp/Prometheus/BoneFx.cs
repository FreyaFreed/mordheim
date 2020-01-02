using System;
using UnityEngine;

namespace Prometheus
{
	[global::System.Serializable]
	public class BoneFx
	{
		public bool active;

		public global::BoneId bone;

		public global::UnityEngine.Vector3 offset;

		public global::UnityEngine.Vector3 rotation;

		public bool rotationWorldSpace;

		public bool lockRotation;

		public float scale;
	}
}

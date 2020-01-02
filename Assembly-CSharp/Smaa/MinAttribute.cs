using System;
using UnityEngine;

namespace Smaa
{
	public sealed class MinAttribute : global::UnityEngine.PropertyAttribute
	{
		public MinAttribute(float min)
		{
			this.min = min;
		}

		public readonly float min;
	}
}

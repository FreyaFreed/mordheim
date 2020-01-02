using System;
using UnityEngine;

namespace Pathfinding
{
	public class AstarEnumFlagAttribute : global::UnityEngine.PropertyAttribute
	{
		public AstarEnumFlagAttribute()
		{
		}

		public AstarEnumFlagAttribute(string name)
		{
			this.enumName = name;
		}

		public string enumName;
	}
}
